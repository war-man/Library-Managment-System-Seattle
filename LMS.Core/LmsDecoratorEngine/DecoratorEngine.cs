﻿using LMS.Contracts;
using LMS.Core.Contracts;
using System;
using System.Diagnostics;

namespace LMS.Core.LmsDecoratorEngine
{
    public class DecoratorEngine : IDecoratorEngine
    {
        private readonly IEngine _engine;
        private readonly IGlobalMessages _messages;
        private readonly ILogoPrinter _logoPrinter;
        private readonly IOutputWriter _writer;

        public DecoratorEngine(IEngine engine, 
                               IGlobalMessages messages, 
                               IOutputWriter writer, 
                               ILogoPrinter logoPrinter)
        {
            _engine = engine;
            _messages = messages;
            _logoPrinter = logoPrinter;
            _writer = writer;
        }
        public void Start()
        {
            var time = new Stopwatch();
            time.Start();
            _logoPrinter.PrintLogo();
            _writer.WriteLine(_messages.WelcomeMessage());
            _engine.Run();
            time.Stop();
            var seconds = time.Elapsed.TotalSeconds;
            _writer.WriteLine(_messages.GetTimeStatisticsMessage());
        }
    }
}
