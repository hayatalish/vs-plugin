﻿using System;
using System.Diagnostics;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;

namespace BlackBerry.DebugEngine
{
    /// <summary>
    /// This class is used only when the user wants to Attach to a running process and it implements:
    /// - IDebugProgramNode2. (http://msdn.microsoft.com/en-ca/library/bb146735.aspx)
    /// - and IDebugProgram2. (http://msdn.microsoft.com/en-us/library/bb147004.aspx)
    /// 
    /// The Debug Engine does not use this class. It uses AD7ProgramNode.cs. This class is used only to represent each of the running 
    /// processes that can be attached to the debug engine. When the debug engine attaches to a running process, an AD7ProgramNode 
    /// class is used to represent the chosen running program. That's why some methods were not implemented.
    /// 
    /// This interface represents a program that can be debugged. 
    /// A running process is viewed as a ProgramNode by VS. 
    /// A debug engine (DE) or a custom port supplier implements this interface to represent a program that can be debugged. 
    /// </summary>
    sealed class AD7ProgramNodeAttach : IDebugProgramNode2, IDebugProgram2
    {
        /// <summary>
        ///  The hosting process. 
        /// </summary>
        private readonly AD7Process _process;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="process"> The process on the target device engine is supposed to attach. </param>
        /// <param name="engineGuid"> The GUID of a debug engine. </param>
        public AD7ProgramNodeAttach(AD7Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");
            if (process.Details == null)
                throw new ArgumentOutOfRangeException("process");

            _process = process;
        }

        #region Properties

        public AD7Process Process
        {
            get { return _process; }
        }

        #endregion

        #region IDebugProgram2 Members

        /// <summary>
        /// Determines if a debug engine (DE) can detach from the program. (http://msdn.microsoft.com/en-us/library/bb161967.aspx)
        /// Not implemented because this class is not used by the debug engine. Read the description of this class at the top.
        /// </summary>
        /// <returns> VSConstants.S_OK. </returns>
        public int CanDetach()
        {
            // The VSNDK debug engine always supports detach
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Requests that the program stop execution the next time one of its threads attempts to run. The debugger calls CauseBreak 
        /// when the user clicks on the pause button in VS. The debugger should respond by entering breakmode. 
        /// (http://msdn.microsoft.com/en-us/library/bb145018.aspx)
        /// Not implemented because this class is not used by the debug engine. Read the description of this class at the top.
        /// </summary>
        /// <returns> VSConstants.S_OK. </returns>
        public int CauseBreak()
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Continues running this program from a stopped state. Any previous execution state (such as a step) is preserved, and 
        /// the program starts executing again. (http://msdn.microsoft.com/en-us/library/bb162148.aspx).
        /// Not implemented because this class is not used by the debug engine. Read the description of this class at the top.
        /// </summary>
        /// <param name="thread"> An IDebugThread2 object that represents the thread. </param>
        /// <returns> VSConstants.S_OK. </returns>
        public int Continue(IDebugThread2 thread)
        {
            // This method is not called by the debug engine.
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Detaches a debug engine from the program. (http://msdn.microsoft.com/en-us/library/bb146228.aspx)
        /// Not implemented because this class is not used by the debug engine. Read the description of this class at the top.
        /// </summary>
        /// <returns> VSConstants.S_OK. </returns>
        public int Detach()
        {
            // This method is not called by the debug engine.
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Retrieves a list of the code contexts for a given position in a source file. Not implemented. 
        /// (http://msdn.microsoft.com/en-us/library/bb145902.aspx)
        /// </summary>
        /// <param name="docPos"> An object representing an abstract position in a source file known to the IDE. </param>
        /// <param name="ppEnum"> Returns an IEnumDebugCodeContexts2 object that contains a list of the code contexts. </param>
        /// <returns> Not implemented. </returns>
        public int EnumCodeContexts(IDebugDocumentPosition2 docPos, out IEnumDebugCodeContexts2 ppEnum)
        {
            ppEnum = null;
            return EngineUtils.NotImplemented();
        }

        /// <summary>
        /// Retrieves a list of the code paths for a given position in a source file. EnumCodePaths is used for the step-into specific 
        /// feature -- right click on the current statement and decide which function to step into. This is not something that the 
        /// VSNDK debug engine supports. (http://msdn.microsoft.com/en-us/library/bb162326.aspx)
        /// </summary>
        /// <param name="hint"> The word under the cursor in the Source or Disassembly view in the IDE. </param>
        /// <param name="start"> An IDebugCodeContext2 object representing the current code context. </param>
        /// <param name="frame"> An IDebugStackFrame2 object representing the stack frame associated with the current breakpoint. </param>
        /// <param name="fSource"> Nonzero (TRUE) if in the Source view, or zero (FALSE) if in the Disassembly view. </param>
        /// <param name="pathEnum"> Returns an IEnumCodePaths2 object containing a list of the code paths. </param>
        /// <param name="safetyContext"> Returns an IDebugCodeContext2 object representing an additional code context to be set as a 
        /// breakpoint in case the chosen code path is skipped. This can happen in the case of a short-circuited Boolean expression, 
        /// for example. </param>
        /// <returns> VSConstants.E_NOTIMPL. </returns>
        public int EnumCodePaths(string hint, IDebugCodeContext2 start, IDebugStackFrame2 frame, int fSource, out IEnumCodePaths2 pathEnum, out IDebugCodeContext2 safetyContext)
        {
            pathEnum = null;
            safetyContext = null;
            return EngineUtils.NotImplemented();
        }

        /// <summary>
        /// Retrieves a list of the modules that this program has loaded and is executing. 
        /// (http://msdn.microsoft.com/en-us/library/bb146980.aspx)
        /// </summary>
        /// <param name="ppEnum"> Returns an IEnumDebugModules2 object that contains a list of the modules. </param>
        /// <returns> VSConstants.S_OK. </returns>
        public int EnumModules(out IEnumDebugModules2 ppEnum)
        {
            // Setting ppEnum to null because we are not adding/working with this feature now. It was causing an error
            // when opening Threads Window and ppEnum = new AD7ModuleEnum(new[] { m_module }).
            ppEnum = null;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Retrieves a list of the threads that are running in the program. (http://msdn.microsoft.com/en-us/library/bb145110.aspx)
        /// Not implemented because this class is not used by the debug engine. Read the description of this class at the top.
        /// </summary>
        /// <param name="ppEnum"> Returns an IEnumDebugThreads2 object that contains a list of the threads. </param>
        /// <returns> If successful, returns S_OK; otherwise, returns an error code. </returns>
        public int EnumThreads(out IEnumDebugThreads2 ppEnum)
        {
            ppEnum = null;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Gets the program's properties. The VSNDK debug engine does not support this. 
        /// (http://msdn.microsoft.com/en-us/library/bb161801.aspx)
        /// </summary>
        /// <param name="ppProperty"> Returns an IDebugProperty2 object that represents the program's properties. </param>
        /// <returns> Not implemented. </returns>
        public int GetDebugProperty(out IDebugProperty2 ppProperty)
        {
            ppProperty = null;
            return EngineUtils.NotImplemented();
        }

        /// <summary>
        /// Gets the disassembly stream for this program or a part of this program.
        /// The sample engine does not support dissassembly so it returns E_NOTIMPL
        /// </summary>
        /// <param name="scope"> Specifies a value from the DISASSEMBLY_STREAM_SCOPE enumeration that defines the scope of the 
        /// disassembly stream.</param>
        /// <param name="codeContext"> An object that represents the position of where to start the disassembly stream. </param>
        /// <param name="disassemblyStream"> Returns an IDebugDisassemblyStream2 object that represents the disassembly stream. </param>
        /// <returns> VSConstants.E_NOTIMPL. </returns>
        public int GetDisassemblyStream(enum_DISASSEMBLY_STREAM_SCOPE scope, IDebugCodeContext2 codeContext, out IDebugDisassemblyStream2 disassemblyStream)
        {
            disassemblyStream = null;
            return EngineUtils.NotImplemented();
        }

        /// <summary>
        /// This method gets the Edit and Continue (ENC) update for this program. A custom debug engine always returns E_NOTIMPL
        /// </summary>
        /// <param name="update"> Returns an internal interface that can be used to update this program. </param>
        /// <returns> VSConstants.S_OK. </returns>
        public int GetENCUpdate(out object update)
        {
            // The VSNDK debug engine does not participate in managed edit & continue.
            update = null;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Retrieves the memory bytes occupied by the program. Not implemented. (http://msdn.microsoft.com/en-us/library/bb145291.aspx)
        /// </summary>
        /// <param name="ppMemoryBytes"> Returns an IDebugMemoryBytes2 object that represents the memory bytes of the program. </param>
        /// <returns> Not implemented. </returns>
        public int GetMemoryBytes(out IDebugMemoryBytes2 ppMemoryBytes)
        {
            ppMemoryBytes = null;
            return EngineUtils.NotImplemented();
        }

        /// <summary>
        /// Gets the name of the program. The name returned by this method is always a friendly, user-displayable name that describes 
        /// the program. (http://msdn.microsoft.com/en-us/library/bb161279.aspx)
        /// </summary>
        /// <param name="programName"> Returns the name of the program. </param>
        /// <returns> VSConstants.S_OK. </returns>
        public int GetName(out string programName)
        {
            programName = _process.Details != null ? _process.Details.Name : string.Empty;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Gets a GUID for this program. A debug engine (DE) must return the program identifier originally passed to the 
        /// IDebugProgramNodeAttach2::OnAttach or IDebugEngine2::Attach methods. This allows identification of the program 
        /// across debugger components. (http://msdn.microsoft.com/en-us/library/bb145581.aspx)
        /// </summary>
        /// <param name="guidProgramId"> Returns the GUID for this program. </param>
        /// <returns> VSConstants.S_OK. </returns>
        public int GetProgramId(out Guid guidProgramId)
        {
            guidProgramId = _process.UID;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Performs a step. This method is deprecated. Use the IDebugProcess3::Step method instead. 
        /// (http://msdn.microsoft.com/en-us/library/bb162134.aspx)
        /// Not implemented because this class is not used by the debug engine. Read the description of this class at the top.
        /// </summary>
        /// <param name="thread"> An IDebugThread2 object that represents the thread being stepped. </param>
        /// <param name="stepKind"> A value from the STEPKIND enumeration that specifies the kind of step. </param>
        /// <param name="step"> A value from the STEPUNIT enumeration that specifies the unit of step. </param>
        /// <returns> If successful, returns S_OK; otherwise, returns an error code. </returns>
        public int Step(IDebugThread2 thread, enum_STEPKIND stepKind, enum_STEPUNIT step)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Terminates the program. (http://msdn.microsoft.com/en-us/library/bb145919.aspx)
        /// Not implemented because this class is not used by the debug engine. Read the description of this class at the top.
        /// </summary>
        /// <returns> VSConstants.S_OK. </returns>
        public int Terminate()
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Writes a dump to a file. Not implemented. (http://msdn.microsoft.com/en-us/library/bb145827.aspx)
        /// </summary>
        /// <param name="type"> A value from the DUMPTYPE enumeration that specifies the type of dump, for example, short or 
        /// long. </param>
        /// <param name="dumpUrl"> The URL to write the dump to. Typically, this is in the form of file://c:\path\filename.ext, 
        /// but may be any valid URL. </param>
        /// <returns> VSConstants.E_NOTIMPL. </returns>
        public int WriteDump(enum_DUMPTYPE type, string dumpUrl)
        {
            // The VSNDK debugger does not support creating or reading mini-dumps.
            return EngineUtils.NotImplemented();
        }

        /// <summary>
        /// Get the process that this program is running in. Not implemented. (http://msdn.microsoft.com/en-us/library/bb145293.aspx)
        /// </summary>
        /// <param name="ppProcess"> Returns the IDebugProcess2 interface that represents the process. </param>
        /// <returns> VSConstants.E_NOTIMPL. </returns>
        public int GetProcess(out IDebugProcess2 ppProcess)
        {
            ppProcess = null;
            return EngineUtils.NotImplemented();
        }

        /// <summary>
        /// Attaches to the program. Not implemented. (http://msdn.microsoft.com/en-us/library/bb161973.aspx)
        /// </summary>
        /// <param name="callback"> An IDebugEventCallback2 object to be used for debug event notification. </param>
        /// <returns> VSConstants.E_NOTIMPL. </returns>
        public int Attach(IDebugEventCallback2 callback)
        {
            return EngineUtils.NotImplemented();
        }

        /// <summary>
        /// Continues running this program from a stopped state. Any previous execution state (such as a step) is cleared, and the 
        /// program starts executing again. Not implemented. (http://msdn.microsoft.com/en-us/library/bb162315.aspx)
        /// Not implemented because this class is not used by the debug engine. Read the description of this class at the top.
        /// </summary>
        /// <returns> VSConstants.S_OK. </returns>
        public int Execute()
        {
            return VSConstants.S_OK;
        }

        #endregion

        #region IDebugProgramNode2 Members

        /// <summary>
        /// Gets the name and GUID of the debug engine running this program. (http://msdn.microsoft.com/en-ca/library/bb146303.aspx)
        /// </summary>
        /// <param name="engineName"> Returns the name of the DE running this program. </param>
        /// <param name="engineGuid"> Returns the GUID of the DE running this program. </param>
        /// <returns> VSConstants.S_OK. </returns>
        public int GetEngineInfo(out string engineName, out Guid engineGuid)
        {
            engineName = _process.Device.Architecture;
            engineGuid = new Guid(AD7Engine.DebugEngineGuid);

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Gets the system process identifier for the process hosting a program. (http://msdn.microsoft.com/en-us/library/bb162159.aspx)
        /// </summary>
        /// <param name="pHostProcessId"> Returns the system process identifier for the hosting process. </param>
        /// <returns> VSConstants.S_OK. </returns>
        public int GetHostPid(AD_PROCESS_ID[] pHostProcessId)
        {
            // According to the MSDN documentation (http://msdn.microsoft.com/en-us/library/bb162159.aspx),
            // it should return the process id of the hosting process, but what is expected is the program ID...
            pHostProcessId[0].ProcessIdType = (uint) enum_AD_PROCESS_ID.AD_PROCESS_ID_GUID;
            pHostProcessId[0].guidProcessId = _process.UID;

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Gets the name of the process hosting a program. (http://msdn.microsoft.com/en-us/library/bb145135.aspx)
        /// </summary>
        /// <param name="type"> A value from the GETHOSTNAME_TYPE enumeration that specifies the type of name to return. </param>
        /// <param name="processName"> Returns the name of the hosting process. </param>
        /// <returns> VSConstants.S_OK. </returns>
        public int GetHostName(enum_GETHOSTNAME_TYPE type, out string processName)
        {
            if (type == enum_GETHOSTNAME_TYPE.GHN_FILE_NAME)
                processName = "(BB-pid = " + _process.Details.ID + ") " + _process.Details.ExecutablePath;
            else
                processName = _process.Details.Name;
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Gets the name of a program. (http://msdn.microsoft.com/en-us/library/bb145928.aspx)
        /// </summary>
        /// <param name="programName"> Returns the name of the program. </param>
        /// <returns> VSConstants.S_OK. </returns>
        int IDebugProgramNode2.GetProgramName(out string programName)
        {
            programName = _process.Details.Name;
            return VSConstants.S_OK;
        }

        #endregion

        #region Deprecated interface methods
        // These methods are not called by the Visual Studio debugger, so they don't need to be implemented

        /// <summary>
        /// DEPRECATED. DO NOT USE. (http://msdn.microsoft.com/en-us/library/bb161399.aspx)
        /// </summary>
        /// <param name="program"> The IDebugProgram2 interface that represents the program to attach to. </param>
        /// <param name="callback"> The IDebugEventCallback2 interface to be used to send debug events to the SDM. </param>
        /// <param name="reason"> A value from the ATTACH_REASON enumeration that specifies the reason for attaching. </param>
        /// <returns> VSConstants.E_NOTIMPL. </returns>
        public int Attach_V7(IDebugProgram2 program, IDebugEventCallback2 callback, uint reason)
        {
            return EngineUtils.NotImplemented();
        }

        /// <summary>
        /// DEPRECATED. DO NOT USE. (http://msdn.microsoft.com/en-us/library/bb161803.aspx)
        /// </summary>
        /// <returns> VSConstants.E_NOTIMPL. </returns>
        public int DetachDebugger_V7()
        {
            return EngineUtils.NotImplemented();
        }

        /// <summary>
        /// DEPRECATED. DO NOT USE. (http://msdn.microsoft.com/en-us/library/bb161297.aspx)
        /// </summary>
        /// <param name="hostMachineName"> Returns the name of the machine in which the program is running. </param>
        /// <returns> VSConstants.E_NOTIMPL. </returns>
        public int GetHostMachineName_V7(out string hostMachineName)
        {
            hostMachineName = null;
            return EngineUtils.NotImplemented();
        }

        #endregion
    }
}
