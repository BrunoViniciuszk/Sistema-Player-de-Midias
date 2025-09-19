# Caminho: api-dotnet/RunCoverage.ps1

# Vai para a pasta de testes
cd ".\ApiDotnet.Tests"

# Cria pasta TestResults se não existir
if (-not (Test-Path ".\TestResults")) {
    New-Item -ItemType Directory -Path ".\TestResults" | Out-Null
}

# Cria pasta coveragereport se não existir
if (-not (Test-Path ".\coveragereport")) {
    New-Item -ItemType Directory -Path ".\coveragereport" | Out-Null
}

# Instala o ReportGenerator como ferramenta local (se ainda não existir)
if (-not (Test-Path ".\.config\dotnet-tools.json")) {
    dotnet new tool-manifest
    dotnet tool install reportgenerator
}

# Roda os testes com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Localiza o arquivo de cobertura gerado pelo Coverlet (formato Cobertura)
$coverageFile = Get-ChildItem -Path ".\TestResults" -Recurse -Filter "*.cobertura.xml" | Select-Object -First 1

if ($coverageFile -eq $null) {
    Write-Error "❌ Nenhum arquivo de cobertura encontrado em TestResults"
    exit 1
}

# Gera relatório HTML usando ReportGenerator
dotnet tool run reportgenerator `
    -reports:$coverageFile.FullName `
    -targetdir:coveragereport `
    -reporttypes:Html

Write-Host "`n✅ Relatório de cobertura gerado em: $(Resolve-Path .\coveragereport)\index.html"
