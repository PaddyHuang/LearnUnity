
CtrlNames = {	
	Login = "LoginCtrl",
	Signup = "SignupCtrl",
	Forget = "ForgetCtrl",
	Course = "CourseCtrl",
	Detial = "DetialCtrl",
	Search = "SearchCtrl",
	Model = "ModelCtrl",
	StudentPractise = "StudentPractiseCtrl",
	StudentUser = "StudentUserCtrl",	
	StudentMessage = "StudentMessageCtrl",	
	Navigation = "NavigationCtrl"
}

PanelNames = {	
	"LoginPanel",
	"SignupPanel",
	"ForgetPanel",
	"CoursePanel",
	"DetialPanel",
	"SearchPanel",
	"ModelPanel",
	"StudentPractisePanel",
	"StudentUserPanel",
	"StudentMessagePanel",
	"NavigationPanel"
}

--协议类型--
ProtocalType = {
	BINARY = 0,
	PB_LUA = 1,
	PBC = 2,
	SPROTO = 3,
}
--当前使用的协议类型--
TestProtoType = ProtocalType.BINARY;

Util = LuaFramework.Util;
AppConst = LuaFramework.AppConst;
LuaHelper = LuaFramework.LuaHelper;
ByteBuffer = LuaFramework.ByteBuffer;

resMgr = LuaHelper.GetResManager();
panelMgr = LuaHelper.GetPanelManager();
soundMgr = LuaHelper.GetSoundManager();
networkMgr = LuaHelper.GetNetManager();
sceneMgr = LuaHelper.GetLoadSceneManager();

WWW = UnityEngine.WWW;
GameObject = UnityEngine.GameObject;