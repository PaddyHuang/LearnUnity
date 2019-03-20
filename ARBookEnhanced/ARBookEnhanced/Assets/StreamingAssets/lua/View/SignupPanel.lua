local transform
local gameObject

SignupPanel = {}
local this = SignupPanel

-- 启动事件
function SignupPanel.Awake(obj)
    gameObject = obj
    transform = obj.transform

    this.InitPanel()
    logWarn("Awake lua--->>"..gameObject.name);
end

function SignupPanel.InitPanel()
    this.panel = transform

    this.phone = transform:Find('Phone'):GetComponent('InputField')
    this.name = transform:Find('Name'):GetComponent('InputField')
    this.account = transform:Find('Account'):GetComponent('InputField')
    this.password = transform:Find('Password'):GetComponent('InputField')
    this.occupation = transform:Find('Occupation'):GetComponent('Dropdown')
    this.school = transform:Find('School'):GetComponent('InputField')
    this.class = transform:Find('Class'):GetComponent('InputField')
    this.inviteCode = transform:Find('InviteCode'):GetComponent('InputField')
    this.email = transform:Find('Email'):GetComponent('InputField')
    this.message = transform:Find('Message'):GetComponent('Text')
    this.signupBtn = transform:Find('SignupBtn').gameObject

    this.donePanel = transform:Find('DonePanel')
    this.doneText = this.donePanel:Find('DoneText'):GetComponent('Text')
    this.doneBtn = this.donePanel:Find('DoneBtn').gameObject
end

--单击事件--
function SignupPanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end