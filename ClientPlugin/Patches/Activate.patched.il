// 534b0ccf
ldarg.0
call VRage.Collections.ListReader`1<Sandbox.Game.Entities.Cube.MyTerminalBlock> Sandbox.Game.Screens.Helpers.MyToolbarItemTerminalGroup::GetBlocks()
stloc.1
ldarg.0
ldarg.0
ldloc.1
ldloca.s 0 (System.Boolean)
call VRage.Collections.ListReader`1<Sandbox.Game.Gui.ITerminalAction> Sandbox.Game.Screens.Helpers.MyToolbarItemTerminalGroup::GetActions(VRage.Collections.ListReader`1<Sandbox.Game.Entities.Cube.MyTerminalBlock> blocks, System.Boolean& genericType)
ldarg.0
call System.String Sandbox.Game.Screens.Helpers.MyToolbarItemActions::get_ActionId()
call Sandbox.Game.Gui.ITerminalAction Sandbox.Game.Screens.Helpers.MyToolbarItemTerminalGroup::FindAction(VRage.Collections.ListReader`1<Sandbox.Game.Gui.ITerminalAction> actions, System.String name)
stloc.2
ldloc.2
brtrue.s L0
ldc.i4.0
ret
L0:
newobj System.Void System.Collections.Generic.List`1<Sandbox.Game.Entities.Cube.MyTerminalBlock>::.ctor()
stloc.3
ldloca.s 1 (VRage.Collections.ListReader`1[Sandbox.Game.Entities.Cube.MyTerminalBlock])
call System.Collections.Generic.Enumerator<Sandbox.Game.Entities.Cube.MyTerminalBlock> VRage.Collections.ListReader`1<Sandbox.Game.Entities.Cube.MyTerminalBlock>::GetEnumerator()
stloc.s 4 (System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock])
[EX_BeginException]
br.s L1
L2:
ldloca.s 4 (System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock])
call virtual Sandbox.Game.Entities.Cube.MyTerminalBlock System.Collections.Generic.Enumerator<Sandbox.Game.Entities.Cube.MyTerminalBlock>::get_Current()
stloc.s 5 (Sandbox.Game.Entities.Cube.MyTerminalBlock)
ldloc.3
ldloc.s 5 (Sandbox.Game.Entities.Cube.MyTerminalBlock)
callvirt virtual System.Void System.Collections.Generic.List`1<Sandbox.Game.Entities.Cube.MyTerminalBlock>::Add(Sandbox.Game.Entities.Cube.MyTerminalBlock item)
L1:
ldloca.s 4 (System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock])
call virtual System.Boolean System.Collections.Generic.Enumerator<Sandbox.Game.Entities.Cube.MyTerminalBlock>::MoveNext()
brtrue.s L2
leave.s L3
[EX_BeginFinally]
ldloca.s 4 (System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock])
constrained. System.Collections.Generic.List`1+Enumerator[Sandbox.Game.Entities.Cube.MyTerminalBlock]
callvirt abstract virtual System.Void System.IDisposable::Dispose()
[EX_EndException]
endfinally
L3:
ldloc.3
ldloc.2
call static System.Void ClientPlugin.ActivatePatch::ApplyAction(System.Collections.Generic.List`1<Sandbox.Game.Entities.Cube.MyTerminalBlock> terminalBlocks, Sandbox.ModAPI.Interfaces.ITerminalAction action)
L8:
ldc.i4.1
ret
