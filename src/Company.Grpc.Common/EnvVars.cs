using System;

namespace Company.Grpc.Common
{
    public static class EnvVars
    {
        public static string SeqAddress(string schema = @"http://", string hostEnv = @"SeqHost", string portEnv = @"SeqPort")
        {
            return $"{schema}{Target(hostEnv, portEnv)}";
        }

        public static string CaCrtPath(string caDirEnv = @"CaDir", string caCrtEnv = @"CaCrt")
        {
            return $"{ParseEnvVar(caDirEnv)}/{ParseEnvVar(caCrtEnv)}";
        }

        public static string ServerPfxPath(string certsDirEnv = @"CertsDir", string serverPfxEnv = @"ServerPfx")
        {
            return $"{ParseEnvVar(certsDirEnv)}/{ParseEnvVar(serverPfxEnv)}";
        }

        public static string CrtPassword(string crtPasswordEnv = @"CrtPassword")
        {
            return ParseEnvVar(crtPasswordEnv);
        }

        public static string ServerCrtPath(string certsDirEnv = @"CertsDir", string serverCrtEnv = @"ServerCrt")
        {
            return $"{ParseEnvVar(certsDirEnv)}/{ParseEnvVar(serverCrtEnv)}";
        }

        public static string ServerKeyPath(string certsDirEnv = @"CertsDir", string serverKeyEnv = @"ServerKey")
        {
            return $"{ParseEnvVar(certsDirEnv)}/{ParseEnvVar(serverKeyEnv)}";
        }

        public static string Target(string hostEnv, string portEnv)
        {
            return $"{ParseHost(hostEnv)}:{ParsePort(portEnv)}";
        }

        public static int LocalPort(string portEnv)
        {
            return ParsePort(portEnv);
        }

        private static string ParseHost(string hostEnv)
        {
            return ParseEnvVar(hostEnv);
        }

        private static int ParsePort(string portEnv)
        {
            return int.Parse(ParseEnvVar(portEnv));
        }

        private static string ParseEnvVar(string varEnv)
        {
            return Environment.GetEnvironmentVariable(varEnv);
        }
    }
}
