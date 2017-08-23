////////////////////////////////////////////////////////////////////////////
// Command.cs - Applies Command pattern of GoF.
//
// Copyright (C) 2016 Terry K. @ GIX Entertainment Inc. & Quasar Inc.
//
////////////////////////////////////////////////////////////////////////////

namespace Quasar.Patterns
{
    public abstract class Command
    {
        public abstract double TimeStamp
        {
            get;
            set;
        }

        public abstract void Execute();
        public abstract void Undo();
    }
}