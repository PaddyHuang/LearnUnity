ForgetCtrl = {}
local this = ForgetCtrl

local transform
local gameObject
local LuaBehaviour

-- 构建函数
function ForgetCtrl.New()    
    this.Awake()
    return this
end

function ForgetCtrl.Awake()    
    panelMgr:CreatePanel('Forget', this.OnCreate)
end

-- 启动事件
function ForgetCtrl.OnCreate(obj)
    gameObject = obj
    transform = obj.transform
    
    LuaBehaviour = transform:GetComponent('LuaBehaviour')    

    LuaBehaviour:AddClick(ForgetPanel.identityBtn, this.OnIdentityClick)
    LuaBehaviour:AddClick(ForgetPanel.confirmBtn, this.OnComfirmClick)
    LuaBehaviour:AddClick(ForgetPanel.doneBtn, this.OnDoneClick)
    
    CtrlManager.CloseCtrl(this)
end

-- 监听事件
function ForgetCtrl.OnIdentityClick(obj)
    print(obj.name)
end

function ForgetCtrl.OnComfirmClick(obj)
    print(obj.name)
end

function ForgetCtrl.OnDoneClick(obj)
    print(obj.name)
end

-- Panel 开关
function ForgetCtrl.Open()
    gameObject:SetActive(true)
end

function ForgetCtrl.Close()
    gameObject:SetActive(false)
end