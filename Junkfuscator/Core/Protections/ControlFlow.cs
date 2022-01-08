using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkfuscator.Core.Protections
{
    public static class ControlFlow
    {
        public class Block { public Block() { Instructions = new List<Instruction>(); } public List<Instruction> Instructions { get; set; } public int Number { get; set; } }

        public static List<Block> GetMethod(MethodDef method) { var blocks = new List<Block>(); var block = new Block(); var id = 0; var usage = 0; block.Number = id; block.Instructions.Add(Instruction.Create(OpCodes.Nop)); blocks.Add(block); block = new Block(); var handlers = new Stack<ExceptionHandler>(); foreach (var instruction in method.Body.Instructions) { foreach (var eh in method.Body.ExceptionHandlers) { if (eh.HandlerStart == instruction || eh.TryStart == instruction || eh.FilterStart == instruction) handlers.Push(eh); } foreach (var eh in method.Body.ExceptionHandlers) { if (eh.HandlerEnd == instruction || eh.TryEnd == instruction) handlers.Pop(); } instruction.CalculateStackUsage(out var stacks, out var pops); block.Instructions.Add(instruction); usage += stacks - pops; if (stacks == 0) { if (instruction.OpCode != OpCodes.Nop) { if ((usage == 0 || instruction.OpCode == OpCodes.Ret) && handlers.Count == 0) { block.Number = ++id; blocks.Add(block); block = new Block(); } } } } return blocks; }


        internal static void Execute(ModuleDef mod)
        {
            foreach (var tDef in mod.Types)
            {
                //if (tDef == mod.GlobalType) continue;
                foreach (var mDef in tDef.Methods)
                {
                    if (mDef.Name.StartsWith("get_") || mDef.Name.StartsWith("set_") || !mDef.HasBody || mDef.IsConstructor) continue;
                    mDef.Body.SimplifyBranches();
                    mDef.Body.SimplifyMacros(mDef.Parameters);
                    var blocks = GetMethod(mDef);
                    var ret = new List<Block>();
                    foreach (var group in blocks)
                    {
                        Random rnd = new Random();
                        ret.Insert(rnd.Next(0, ret.Count), group);
                    }
                    blocks = ret;
                    mDef.Body.Instructions.Clear();
                    var local = new Local(mDef.Module.CorLibTypes.Int32);
                    mDef.Body.Variables.Add(local);
                    var target = Instruction.Create(OpCodes.Nop);
                    var instr = Instruction.Create(OpCodes.Br, target);
                    var instructions = new List<Instruction> { Instruction.Create(OpCodes.Ldc_I4, 1) };
                    foreach (var instruction in instructions)
                        mDef.Body.Instructions.Add(instruction);
                    mDef.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
                    mDef.Body.Instructions.Add(Instruction.Create(OpCodes.Br, instr));
                    mDef.Body.Instructions.Add(target);
                    foreach (var block in blocks.Where(block => block != blocks.Single(x => x.Number == blocks.Count - 1)))
                    {
                        mDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
                        var instructions1 = new List<Instruction> { Instruction.Create(OpCodes.Ldc_I4, block.Number) };
                        foreach (var instruction in instructions1)
                            mDef.Body.Instructions.Add(instruction);
                        mDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
                        var instruction4 = Instruction.Create(OpCodes.Nop);
                        mDef.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction4));

                        foreach (var instruction in block.Instructions)
                            mDef.Body.Instructions.Add(instruction);

                        var instructions2 = new List<Instruction> { Instruction.Create(OpCodes.Ldc_I4, block.Number + 1) };
                        foreach (var instruction in instructions2)
                            mDef.Body.Instructions.Add(instruction);

                        mDef.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local));
                        mDef.Body.Instructions.Add(instruction4);
                    }
                    mDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local));
                    var instructions3 = new List<Instruction> { Instruction.Create(OpCodes.Ldc_I4, blocks.Count - 1) };
                    foreach (var instruction in instructions3)
                        mDef.Body.Instructions.Add(instruction);
                    mDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ceq));
                    mDef.Body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instr));
                    mDef.Body.Instructions.Add(Instruction.Create(OpCodes.Br, blocks.Single(x => x.Number == blocks.Count - 1).Instructions[0]));
                    mDef.Body.Instructions.Add(instr);

                    foreach (var lastBlock in blocks.Single(x => x.Number == blocks.Count - 1).Instructions)
                        mDef.Body.Instructions.Add(lastBlock);
                }
            }
        }
    }
}
