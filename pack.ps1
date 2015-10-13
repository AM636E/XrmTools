ForEach ($item in (Get-ChildItem -Path "./src" -Directory)) {
	Write-Host "$($item.FullName)\project.json"
	if(Test-Path "$($item.FullName)\project.json") {
		Set-Location $item.FullName
		if(Test-Path "./bin") {
			Remove-Item -Recurse -Force "./bin"
		}
		dnu restore > $null
		dnu pack > $null
		ForEach($file in (Get-ChildItem -Path "./bin/Debug" -File)) {
			if($file.Extension.Equals(".nupkg") -and -not($file.Name.Contains("symbols")) ) {
				
			}
		}
		Set-Location ../../
	}
}