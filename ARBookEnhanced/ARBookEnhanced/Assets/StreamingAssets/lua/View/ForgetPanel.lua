local transform
local gameObject

ForgetPanel = {}
local this = ForgetPanel

-- 启动事件
function ForgetPanel.Awake(obj)
    gameObject = obj
    transform = obj.transform

    this.InitPanel()
    logWarn("Awake lua--->>"..gameObject.name);
end

function ForgetPanel.InitPanel()
    this.phone = transform:Find('Phone'):GetComponent('InputField')    
    this.identityCode = transform:Find('IdentityCode'):GetComponent('InputField')
    this.identityBtn = transform:Find('IdentityBtn').gameObject
    this.newPassword = transform:Find('NewPassword'):GetComponent('InputField')
    this.message = transform:Find('Message'):GetComponent('Text')
    this.confirmBtn = transform:Find('ComfirmBtn').gameObject

    this.donePanel = transform:Find('DonePanel')    
    this.doneBtn = this.donePanel:Find('DoneBtn').gameObject
end

--单击事件--
function SignupPanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end