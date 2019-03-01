ModelCtrl = {}
local this = ModelCtrl

local transform
local gameObject
local LuaBehaviour

-- 构建函数
function ModelCtrl.New()   
    this.Awake() 
    return this
end

function ModelCtrl.Awake()    
    panelMgr:CreatePanel('Model', this.OnCreate)
end

-- 启动事件
function ModelCtrl.OnCreate(obj)
    gameObject = obj
    transform = obj.transform
    
    LuaBehaviour = transform:GetComponent('LuaBehaviour')    

    LuaBehaviour:AddClick(ModelPanel.backBtn, this.OnBackClick)
           
    CtrlManager.CloseCtrl(this)
end

-- 点击事件
function ModelCtrl.OnBackClick(obj)
    print(obj.name)
end


-- Panel 开关
function ModelCtrl.Open()
    gameObject:SetActive(true)
end

function ModelCtrl.Close()
    gameObject:SetActive(false)
end