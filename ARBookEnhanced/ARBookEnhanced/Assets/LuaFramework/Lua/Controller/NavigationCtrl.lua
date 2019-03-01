NavigationCtrl = {}
local this = NavigationCtrl

local transform
local gameObject
local LuaBehaviour

-- 构建函数
function NavigationCtrl.New()   
    this.Awake() 
    return this
end

function NavigationCtrl.Awake()    
    panelMgr:CreatePanel('Navigation', this.OnCreate)
end

-- 启动事件
function NavigationCtrl.OnCreate(obj)
    gameObject = obj
    transform = obj.transform
    transform.localPosition = Vector3(0, -565, 0)

    LuaBehaviour = transform:GetComponent('LuaBehaviour')    

    LuaBehaviour:AddClick(NavigationPanel.courseBtn, this.OnCourseClick)
    LuaBehaviour:AddClick(NavigationPanel.practiseBtn, this.OnPractiseClick)
    LuaBehaviour:AddClick(NavigationPanel.messageBtn, this.OnMessageClick)
    LuaBehaviour:AddClick(NavigationPanel.userBtn, this.OnUserClick)
    
end

-- 点击事件
function NavigationCtrl.OnCourseClick(obj)
    CtrlManager.OpenCtrl(CtrlNames.Course)
end

function NavigationCtrl.OnPractiseClick(obj)
    CtrlManager.OpenCtrl(CtrlNames.StudentPractise)
end

function NavigationCtrl.OnMessageClick(obj)
    CtrlManager.OpenCtrl(CtrlNames.StudentMessage)
end

function NavigationCtrl.OnUserClick(obj)
    CtrlManager.OpenCtrl(CtrlNames.StudentUser)
end


-- Panel 开关
function NavigationCtrl.Open()
    NavigationPanel.panel.gameObject:SetActive(true)
end

function NavigationCtrl.Close()
    NavigationPanel.panel.gameObject:SetActive(false)
end