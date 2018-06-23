Set-StrictMode -Version 2.0

$CrtPassword = "Password123!"
$Days = 3650
$CaKey = "ca.key"
$CaCrt = "ca.crt"
$ServerKey = "server.key"
$ServerCsr = "server.csr"
$ServerCrt = "server.crt"
$ServerPfx = "server.pfx"

$CertsSuffix = "certs"
$RestApi="company.api.rest.servicegrpc"
$MembershipManager="company.manager.membership.servicegrpc"
$RegistrationEngine="company.engine.registration.servicegrpc"
$UserAccess="company.access.user.servicegrpc"

$CaDir = "ca.$CertsSuffix"

############################################

function createServerCerts($CertsDir, $CommonName) {
    If(!(test-path $CertsDir))
    {
          New-Item -ItemType Directory -Force -Path $CertsDir      
    }

    Write-Output "Generate server key:"
    openssl genrsa -passout pass:$CrtPassword -des3 -out $CertsDir/$ServerKey $Days
    
    Write-Output "Generate server signing request:"
    openssl req -passin pass:$CrtPassword -new -key $CertsDir/$ServerKey -out $CertsDir/$ServerCsr -subj "/C=US/ST=CA/L=Cupertino/O=YourCompany/OU=YourApp/CN=$CommonName"
    
    Write-Output "Generate self-sign server certificate:"
    openssl x509 -req -passin pass:$CrtPassword -days $Days -in $CertsDir/$ServerCsr -CA $CaDir/$CaCrt -CAkey $CaDir/$CaKey -set_serial 01 -out $CertsDir/$ServerCrt

    Write-Output "Remove passphrase from server key:"
    openssl rsa -passin pass:$CrtPassword -in $CertsDir/$ServerKey -out $CertsDir/$ServerKey
        
    Write-Output "Generate server pfx:"
    openssl pkcs12 -export -out $CertsDir/$ServerPfx -inkey $CertsDir/$ServerKey -in $CertsDir/$ServerCrt -certfile $CertsDir/$ServerCrt -passout pass:$CrtPassword
  }

############################################

If(!(test-path $CaDir))
{
      New-Item -ItemType Directory -Force -Path $CaDir      
}

    
Write-Output "Generate CA key:"
openssl genrsa -passout pass:$CrtPassword -des3 -out $CaDir/$CaKey 4096

Write-Output "Generate CA certificate:"
openssl req -passin pass:$CrtPassword -new -x509 -days $Days -key $CaDir/$CaKey -out $CaDir/$CaCrt -subj "/C=US/ST=CA/L=Cupertino/O=YourCompany/OU=YourApp/CN=MyRootCA"

createServerCerts "$RestApi.$CertsSuffix" $RestApi
createServerCerts "$MembershipManager.$CertsSuffix" $MembershipManager
createServerCerts "$RegistrationEngine.$CertsSuffix" $RegistrationEngine
createServerCerts "$UserAccess.$CertsSuffix" $UserAccess

############################################