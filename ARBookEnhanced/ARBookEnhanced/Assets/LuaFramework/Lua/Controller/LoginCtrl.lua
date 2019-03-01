require "Common/functions"

LoginCtrl = {}
local this = LoginCtrl

local transform
local gameObject
local LuaBehaviour

-- 构建函数
function LoginCtrl.New()   
    this.Awake() 
    return this
end

function LoginCtrl.Awake()    
    panelMgr:CreatePanel('Login', this.OnCreate)
end

-- 启动事件
function LoginCtrl.OnCreate(obj)
    gameObject = obj
    transform = obj.transform
    
    LuaBehaviour = transform:GetComponent('LuaBehaviour')    

    LuaBehaviour:AddClick(LoginPanel.loginBtn, this.OnLoginClick)
    LuaBehaviour:AddClick(LoginPanel.forgetBtn, this.OnForgetClick)
    LuaBehaviour:AddClick(LoginPanel.signupBtn, this.OnSignupClick)

end

-- 点击事件
function LoginCtrl.OnLoginClick()
    -- 检查各项是否输入正确
    

    print(UserManager.Instance:LoginUser(LoginPanel.account.text, LoginPanel.password.text))
    sceneMgr.LoadSceneByIndex(1)    -- 加载学生场景
end

function LoginCtrl.OnForgetClick()
    CtrlManager.OpenCtrl(CtrlNames.Forget)
end

function LoginCtrl.OnSignupClick()
    CtrlManager.OpenCtrl(CtrlNames.Signup)
end


-- Panel 开关
function LoginCtrl.Open()
    LoginPanel.panel.gameObject:SetActive(true)
end

function LoginCtrl.Close()
    LoginPanel.panel.gameObject:SetActive(false)
end