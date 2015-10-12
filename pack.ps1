ForEach ($item in (Get-ChildItem -Path "./src" -Directory)) {
	Write-Host "$($item.FullName)\project.json"
	if(Test-Path "$($item.FullName)\project.json") {
		Set-Location $item.FullName
		Remove-Item -Recurse -Force "./bin"
		dnu restore > $null
		dnu pack --configuration Release > $null
		ForEach($file in (Get-ChildItem -Path "./bin/Release" -File)) {
			if($file.Extension.Equals(".nupkg") -and -not($file.Name.Contains("symbols")) ) {
				nuget push $file.FullName -s http://80.4.97.108:9055/ apikey
			}
		}
		Set-Location ../../
	}
}