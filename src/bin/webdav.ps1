function main {
  $webdav = new-object WebDav.WebDavClient("https://webdav.yandex.ru")
  $webdav.SetAuthorization("ezyuzin@yandex.ru", "nydagqjdigacoinr");
  $files = $webdav.GetFiles("/Backups/Docs");
  $files | ForEach-Object { Write-Host $_ }

}

function Get-ScriptDirectory {
	$invocation0 = (Get-Variable MyInvocation -Scope 0);
	$location = [System.IO.Path]::GetDirectoryName($invocation0.Value.ScriptName);
	return $location;
}

$location = Get-ScriptDirectory;
$bin = [System.Reflection.Assembly]::LoadFile("$location/YandexDiscSync.dll");
main;

