require "Common/define"

require "Controller/ForgetCtrl"
require "Controller/SignupCtrl"
require "Controller/LoginCtrl"
require "Controller/CourseCtrl"
require "Controller/DetialCtrl"
require "Controller/SearchCtrl"
require "Controller/ModelCtrl"
require "Controller/StudentPractiseCtrl"
require "Controller/StudentUserCtrl"
require "Controller/StudentMessageCtrl"
require "Controller/NavigationCtrl"

CtrlManager = {};
local this = CtrlManager;
local ctrlList = {};	--控制器列表--

function CtrlManager.InitScene(sceneName)
	logWarn("CtrlManager.Init----->>>");
	
	if sceneName == 'LoginScene' then
		ctrlList[CtrlNames.Forget] = ForgetCtrl.New();
		ctrlList[CtrlNames.Signup] = SignupCtrl.New();
		ctrlList[CtrlNames.Login] = LoginCtrl.New();	
		this.currentCtrl = ctrlList[CtrlNames.Login];	-- 初始控制器
	end

	if sceneName == 'StudentScene' then		
		ctrlList[CtrlNames.Detial] = DetialCtrl.New();
		ctrlList[CtrlNames.Search] = SearchCtrl.New();
		ctrlList[CtrlNames.Model] = ModelCtrl.New();
		ctrlList[CtrlNames.Course] = CourseCtrl.New();
		ctrlList[CtrlNames.StudentPractise] = StudentPractiseCtrl.New();
		ctrlList[CtrlNames.StudentUser] = StudentUserCtrl.New();
		ctrlList[CtrlNames.StudentMessage] = StudentMessageCtrl.New();
		ctrlList[CtrlNames.Navigation] = NavigationCtrl.New();

		this.currentCtrl = ctrlList[CtrlNames.Course];	-- 初始控制器
	end
	
	-- return this;
end

--添加控制器--
function CtrlManager.AddCtrl(ctrlName, ctrlObj)
	ctrlList[ctrlName] = ctrlObj;
end

--获取控制器--
function CtrlManager.GetCtrl(ctrlName)
	return ctrlList[ctrlName];
end

--移除控制器--
function CtrlManager.RemoveCtrl(ctrlName)
	ctrlList[ctrlName] = nil;
end

--关闭控制器--
function CtrlManager.Close()
	logWarn('CtrlManager.Close---->>>');
end

-- 关闭控制器
function CtrlManager.CloseCtrl(ctrl)
	if ctrl.Close ~= nil then
		ctrl.Close()
	end
end

-- 关闭当前控制器
function CtrlManager.CloseCurrentCtrl()
	if this.currentCtrl.Close ~= nil then
		this.currentCtrl.Close()
	end
end

-- 开启一个控制器，关闭前一个控制器
function CtrlManager.OpenCtrl(ctrlName)
	this.CloseCurrentCtrl()
	ctrlList[ctrlName].Open()
	this.currentCtrl = ctrlList[ctrlName]	
end
