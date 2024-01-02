function main {
  $location = (Get-ScriptDirectory);
  $root = "$($location)/Docs";
  $creds = [xml]([IO.File]::ReadAllText("$location/pwd.xml"));

  $7zip = "$location/lib/7z/7z.exe"
  $backupDir = "$location/zip";
  $source = "$location/data";

  $yaDisc = $creds.SelectSingleNode("/data/yandexDisc");
	
  $backupDisc = @{username = $yaDisc.username; password = $yaDisc.password; location = "/Backups/Temp" };
  $pwd7z = $creds.SelectSingleNode("/data/backup7z").password;
    
  write-host "Docs archiving started ...";
  $null = [System.Reflection.Assembly]::LoadFrom("$location/lib/YandexDiscSync.dll")
  $timestamp = [DateTime]::Now.ToString("yyMMdd_HHmmss");

  if ((test-path $backupDir) -eq $false) {
    $null = mkdir $backupDir;
  } 

  backup -dir $source
}

function backup($dir) {
  $prev = @{};
  $backupName = "content-$($timestamp)";

  if (test-path "$location/zip/state.json") {
    foreach($line in [IO.File]::ReadLines("$location/zip/state.json")) {
      $info = ConvertFrom-Json -InputObject $line -AsHashtable;
      $name = $info['name'];
      $prev[$name] = $info;
    }
    $backupName = "$($backupName)_inc";
  }

  $state = @{};
  foreach($file in (Get-ChildItem -File -Recurse -LiteralPath $dir)) {
    $name = $file.FullName.Substring($dir.Length + 1).Replace("\", "/");
    $state[$name] = @{ 
      name = $name;
      timestamp = $file.LastAccessTimeUtc.Ticks;
      length = $file.Length;
      md5 = (Get-FileHash -Algorithm SHA256 -Path $file.FullName).Hash.ToLower();
    };
  }

  # p	File exists in archive, but is not matched with wildcard
  # q	File exists in archive, but doesn't exist on disk
  # r	File doesn't exist in archive, but exists on disk
  # x	File in archive is newer than the file on disk
  # y	File in archive is older than the file on disk
  # z	File in archive is same as the file on disk
  # w	Can not be detected what file is newer (times are the same, sizes are different) 

  # 0	Ignore file (don't create item in new archive for this file)
  # 1	Copy file (copy from old archive to new)
  # 2	Compress (compress file from disk to new archive)
  # 3	Create Anti-item (item that will delete file or directory during extracting). This feature is supported only in 7z format.

  $deleted = 0;
  cd $source
  foreach($name in $prev.Keys) {
    if ($state.ContainsKey($name) -ne $true) {
      "" | out-file -NoNewline "$location/tmp.tmp"; 
      &"$7zip" a "$backupDir/$backupName.7z" "$location/tmp.tmp"
      &"$7zip" rn "$backupDir/$backupName.7z" "tmp.tmp" "$name"
      remove-item "$location/tmp.tmp"
      &"$7zip" u "$backupDir/$backupName.7z" -up1q3r0x1y1z1w1 -y -mx5 -t7z -- "$name"
      $deleted += 1;
    }
  }

  $modified = 0;
  if (test-path "$location/zip/modified.txt") {
    remove-item "$location/zip/modified.txt";
  }

  foreach($name in $state.Keys) {
    if (($prev.ContainsKey($name) -eq $false) -or ($prev[$name]['md5'] -ne $state[$name]['md5'])) {
      "$name" | Out-File -Append -LiteralPath "$location/zip/modified.txt";
      $modified += 1;
    }
  }
  
  if ($modified -ne 0) {
    &"$7zip" a "$backupDir/$backupName.7z" -y -mx9 -t7z -scsUTF-8 -bb3 "@$location/zip/modified.txt"
    remove-item "$location/zip/modified.txt"
  }

  if (($deleted + $modified) -ne 0) {
    "" | Out-File -NoNewline -LiteralPath "$location/zip/state.json";
    foreach($name in $state.Keys) {
      (ConvertTo-Json -Compress -InputObject $state[$name]) | Out-File -Append -LiteralPath "$location/zip/state.json"
    }

    try {
      &"$7zip" u "$backupDir/$backupName.7z" "$source/*" -u- "-up1q3r0x1y1z1w1!$backupDir/$($backupName)_enc.7z" -y -mx9 -t7z -p"$pwd7z" -mhe
      saveToYandexDisc -localFile "$backupDir/$($backupName)_enc.7z" -webdavFile "$($backupDisc.location)/$($backupName).7z"
    }
    finally {
      if (test-path "$backupDir/$($backupName)_enc.7z") {
        remove-item "$backupDir/$($backupName)_enc.7z";
      }
    }
  }
  else {
    write-host "... no changes";
  }
}

function saveToYandexDisc($webdavFile, $localFile) {
  return;

  $webdav = new-object WebDav.WebDavClient("https://webdav.yandex.ru");
  $webdav.SetAuthorization($backupDisc.username, $backupDisc.password);
	
  $webdavFolder = [IO.Path]::GetDirectoryName($webdavFile).Replace('/', "/");
  $filename = [IO.Path]::GetFileName($webdavFile);
	
  Write-Host $webdavFolder
  $webdav.CreateDirectory($webdavFolder);
  $ix = 0;
  $webdavFile = $filename;
  while ($true) {
    if ($webdav.IsFileExist("$webdavFolder/$webdavFile") -eq $false) {
      break;
    }
    $webdavFile = [IO.Path]::GetFileNameWithoutExtension($filename);
    $webdavFile = $webdavFile + "_$ix"
    $webdavFile = $webdavFile + [IO.Path]::GetExtension($filename);
		
    $ix = $ix + 1;
    Sleep -Milliseconds 5
  }			

  $cursorLeft = $null;
  try {
    $cursorLeft = [Console]::CursorLeft;
    $cursorTop = [Console]::CursorTop;
  }
  catch {
  }
	
  $lastProgress = "";
  $webdav.Upload("$webdavFolder/$webdavFile", $localFile, [Webdav.WebDavOperationCallback] {
      param($progress)
		
      $value = $progress.Progress.ToString("0");
      if ($cursorLeft -ne $null) {
        $value = $progress.Progress.ToString("0.0");
        [Console]::CursorTop = $cursorTop;
        [Console]::CursorLeft = $cursorLeft;
      }
		
      if ($lastProgress -ne $value) {
        $lastProgress = $value;
        Write-Host "($value%) Upload to $webdavFolder/$webdavFile ...";	
      }
    });
}

function AssertLastExitCode([string]$message) {
  $hresult = $LASTEXITCODE;
  if ($hresult -ne 0) {
    throw New-Object System.ApplicationException("$message - errorcCode: $hresult");
  }
}

function Get-ScriptDirectory {
  $invocation0 = (Get-Variable MyInvocation -Scope 0);
  $location = [System.IO.Path]::GetDirectoryName($invocation0.Value.ScriptName);
  return $location;
}

try { 
  main;
  exit 0;
}
catch {
  Write-Host -BackgroundColor Red -ForegroundColor White $_;
  exit 1;
}