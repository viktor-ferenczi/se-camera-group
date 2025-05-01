// 557fc120
ldarg.0
call VRage.Collections.ListReader`1<Sandbox.Game.Entities.Cube.MyTerminalBlock> Sandbox.Game.Screens.Helpers.MyToolbarItemTerminalGroup::GetBlocks()
stloc.1
ldarg.0
ldloc.1
ldloca.s 0 (System.Boolean)
call VRage.Collections.ListReader`1<Sandbox.Game.Gui.ITerminalAction> Sandbox.Game.Screens.Helpers.MyToolbarItemTerminalGroup::GetActionsWithGenericDuplicates(VRage.Collections.ListReader`1<Sandbox.Game.Entities.Cube.MyTerminalBlock> blocks, System.Boolean& genericType)
ldarg.0
call System.String Sandbox.Game.Screens.Helpers.MyToolbarItemActions::get_ActionId()
call static System.Collections.Generic.List`1<Sandbox.Game.Gui.ITerminalAction> Sandbox.Game.Screens.Helpers.MyToolbarItemTerminalGroup::FindActions(VRage.Collections.ListReader`1<Sandbox.Game.Gui.ITerminalAction> actions, System.String name)
stloc.2
ldloc.2
brfalse.s L0
ldloc.2
callvirt virtual System.Int32 System.Collections.Generic.List`1<Sandbox.Game.Gui.ITerminalAction>::get_Count()
brtrue.s L1
L0:
ldc.i4.0
ret
L1:
newobj System.Void System.Collections.Generic.List`1<Sandbox.Game.Entities.Cube.MyTerminalBlock>::.ctor()
stloc.3
ldloca.s 1 (VRage.Collections.ListReader`1[Sandbox.Game.Entities.Cube.MyTerminalBlock])
call System.Collections.Generic.Enumerator<Sandbox.Game.Entities.Cube.MyTerminalBlock> VRage.Collections.ListReader`1<Sandbox.Game.Entities.Cube.MyTerminalBlock>::GetEnumerator()
stloc.s 4 (System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock])
[EX_BeginException]
br.s L2
L3:
ldloca.s 4 (System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock])
call virtual Sandbox.Game.Entities.Cube.MyTerminalBlock System.Collections.Generic.Enumerator<Sandbox.Game.Entities.Cube.MyTerminalBlock>::get_Current()
stloc.s 5 (Sandbox.Game.Entities.Cube.MyTerminalBlock)
ldloc.3
ldloc.s 5 (Sandbox.Game.Entities.Cube.MyTerminalBlock)
callvirt virtual System.Void System.Collections.Generic.List`1<Sandbox.Game.Entities.Cube.MyTerminalBlock>::Add(Sandbox.Game.Entities.Cube.MyTerminalBlock item)
L2:
ldloca.s 4 (System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock])
call virtual System.Boolean System.Collections.Generic.Enumerator<Sandbox.Game.Entities.Cube.MyTerminalBlock>::MoveNext()
brtrue.s L3
leave.s L4
[EX_BeginFinally]
ldloca.s 4 (System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock])
constrained. System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock]
callvirt abstract virtual System.Void System.IDisposable::Dispose()
[EX_EndException]
endfinally
L4:
ldloc.3
ldloc.2
call static System.Boolean ClientPlugin.Patches.ActivatePatch::HandleViewAction(System.Collections.Generic.List`1<Sandbox.Game.Entities.Cube.MyTerminalBlock> terminalBlocks, System.Collections.Generic.List`1<Sandbox.ModAPI.Interfaces.ITerminalAction> actions)
brtrue L12
ldloc.3
callvirt System.Collections.Generic.Enumerator<Sandbox.Game.Entities.Cube.MyTerminalBlock> System.Collections.Generic.List`1<Sandbox.Game.Entities.Cube.MyTerminalBlock>::GetEnumerator()
stloc.s 4 (System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock])
[EX_BeginException]
br.s L5
L10:
ldloca.s 4 (System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock])
call virtual Sandbox.Game.Entities.Cube.MyTerminalBlock System.Collections.Generic.Enumerator<Sandbox.Game.Entities.Cube.MyTerminalBlock>::get_Current()
stloc.s 6 (Sandbox.Game.Entities.Cube.MyTerminalBlock)
ldloc.s 6 (Sandbox.Game.Entities.Cube.MyTerminalBlock)
brfalse.s L6
ldloc.s 6 (Sandbox.Game.Entities.Cube.MyTerminalBlock)
callvirt virtual System.Boolean Sandbox.Game.Entities.MyCubeBlock::get_IsFunctional()
brfalse.s L7
ldarg.0
ldloc.2
ldloc.s 6 (Sandbox.Game.Entities.Cube.MyTerminalBlock)
call Sandbox.Game.Gui.ITerminalAction Sandbox.Game.Screens.Helpers.MyToolbarItemTerminalGroup::GetMatchingGenericAction(System.Collections.Generic.List`1<Sandbox.Game.Gui.ITerminalAction> actions, VRage.Game.ModAPI.IMyCubeBlock block)
dup
brtrue.s L8
pop
br.s L9
L8:
ldloc.s 6 (Sandbox.Game.Entities.Cube.MyTerminalBlock)
ldarg.0
ldfld System.Collections.Generic.List`1<Sandbox.ModAPI.Ingame.TerminalActionParameter> Sandbox.Game.Screens.Helpers.MyToolbarItemActions::m_parameters
call static VRage.Collections.ListReader`1<Sandbox.ModAPI.Ingame.TerminalActionParameter> VRage.Collections.ListReader`1<Sandbox.ModAPI.Ingame.TerminalActionParameter>::op_Implicit(System.Collections.Generic.List`1<Sandbox.ModAPI.Ingame.TerminalActionParameter> list)
callvirt abstract virtual System.Void Sandbox.Game.Gui.ITerminalAction::Apply(Sandbox.Game.Entities.Cube.MyTerminalBlock block, VRage.Collections.ListReader`1<Sandbox.ModAPI.Ingame.TerminalActionParameter> parameters)
L5:
L6:
L7:
L9:
ldloca.s 4 (System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock])
call virtual System.Boolean System.Collections.Generic.Enumerator<Sandbox.Game.Entities.Cube.MyTerminalBlock>::MoveNext()
brtrue.s L10
leave.s L11
[EX_BeginFinally]
ldloca.s 4 (System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock])
constrained. System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock]
callvirt abstract virtual System.Void System.IDisposable::Dispose()
[EX_EndException]
endfinally
L11:
L12:
ldc.i4.1
ret
