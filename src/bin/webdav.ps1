function main {
  $bin = [System.Reflection.Assembly]::LoadFile("$location\YandexDiscSync.dll");
  write-host $bin;
  $webdav = new-object WebDav.WebDavClient;
  $webdav.SetAuthorization("username@yandex.ru", "12345");
  $files = $webdav.GetFiles("/");
}

function Get-ScriptDirectory {
	$invocation0 = (Get-Variable MyInvocation -Scope 0);
	$location = [System.IO.Path]::GetDirectoryName($invocation0.Value.ScriptName);
	return $location;
}

$location = Get-ScriptDirectory;
$bin = [System.Reflection.Assembly]::LoadFile("$location\YandexDiscSync.dll");
main;

