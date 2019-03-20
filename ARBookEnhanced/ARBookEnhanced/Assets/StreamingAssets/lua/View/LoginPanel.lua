local transform
local gameObject

LoginPanel = {}
local this = LoginPanel

-- 启动事件
function LoginPanel.Awake(obj)
    gameObject = obj
    transform = obj.transform

    this.InitPanel()
    logWarn("Awake lua--->>"..gameObject.name);
end

function LoginPanel.InitPanel()
    this.panel = transform

    this.account = transform:Find('Account'):GetComponent('InputField')
    this.password = transform:Find('Password'):GetComponent('InputField')
    this.message = transform:Find('Message'):GetComponent('Text')
    this.loginBtn = transform:Find('LoginBtn').gameObject
    this.forgetBtn = transform:Find('ForgetBtn').gameObject
    this.signupBtn = transform:Find('SignupBtn').gameObject    
end

--单击事件--
function LoginPanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end