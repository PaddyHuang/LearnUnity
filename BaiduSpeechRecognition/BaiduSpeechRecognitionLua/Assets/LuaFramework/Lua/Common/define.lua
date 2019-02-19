
CtrlNames = {
	-- Prompt = "PromptCtrl",
	-- Message = "MessageCtrl",
	Recognise = "RecognisePanelCtrl"
}

PanelNames = {
	-- "PromptPanel",	
	-- "MessagePanel",
	"RecognisePanel"
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

WWW = UnityEngine.WWW;
GameObject = UnityEngine.GameObject;
AudioSource = UnityEngine.AudioSource;
Microphone = UnityEngine.Microphone;

Url = "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&"