using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateDataLib.Schema.Generator
{
    public interface IGeneratorWriter : IDisposable
    {
        UInt32 PlatformType();
        string InfoFileName();
        string InfoFilePath();
        void PrepareInfo(MigrateOptions buildOptions);
        void PrepareCode(MigrateOptions buildOptions);
        void OpenCode(UInt32 sourceType, string codeFilePath);
        void CloseCode();
        void WriteInfo(string infoText);
        void WriteInfo(string format, params object[] args);
        void WriteInfoLine(string infoText);
        void WriteInfoLine(string format, params object[] args);
        void WriteCodeInBase64Line(string codeText);
        void WriteCode(string codeText);
        void WriteCode(string format, params object[] args);
        void WriteCodeLine(string codeText);
        void ExecuteCodeLine(string codeText, string infoName);
        long GetScriptCount(string countQuery);
        void DefaultCodeLine(string codeText, string infoName);
    }
}
