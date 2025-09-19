cd ".\ApiDotnet.Tests"

if (-not (Test-Path ".\TestResults")) {
    New-Item -ItemType Directory -Path ".\TestResults" | Out-Null
}

if (-not (Test-Path ".\coveragereport")) {
    New-Item -ItemType Directory -Path ".\coveragereport" | Out-Null
}

if (-not (Test-Path ".\.config\dotnet-tools.json")) {
    dotnet new tool-manifest
    dotnet tool install reportgenerator
}

dotnet test --collect:"XPlat Code Coverage"

$coverageFile = Get-ChildItem -Path ".\TestResults" -Recurse -Filter "*.cobertura.xml" | Select-Object -First 1

if ($coverageFile -eq $null) {
    Write-Error "❌ Nenhum arquivo de cobertura encontrado em TestResults"
    exit 1
}

dotnet tool run reportgenerator `
    -reports:$coverageFile.FullName `
    -targetdir:coveragereport `
    -reporttypes:Html

Write-Host "`n✅ Relatório de cobertura gerado em: $(Resolve-Path .\coveragereport)\index.html"
