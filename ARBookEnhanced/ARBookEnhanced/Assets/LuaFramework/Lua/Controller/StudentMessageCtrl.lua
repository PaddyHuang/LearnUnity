StudentMessageCtrl = {}
local this = StudentMessageCtrl

local transform
local gameObject
local LuaBehaviour

-- 构建函数
function StudentMessageCtrl.New()   
    this.Awake() 
    return this
end

function StudentMessageCtrl.Awake()    
    panelMgr:CreatePanel('StudentMessage', this.OnCreate)
end

-- 启动事件
function StudentMessageCtrl.OnCreate(obj)
    gameObject = obj
    transform = obj.transform
    
    LuaBehaviour = transform:GetComponent('LuaBehaviour')    

    -- 加载课程，之后改成按Json动态添加
    resMgr:LoadPrefab('MessageItem', {'MessageItem'}, this.OnLoadMessageItemFinish) 

    CtrlManager.CloseCtrl(this)
end

-- 点击事件
function StudentMessageCtrl.OnStudentMessageSelect(obj)
    print(obj)
end

-- 加载回调
function StudentMessageCtrl.OnLoadMessageItemFinish(obj)
    this.message = GameObject.Instantiate(obj[0])
    
    this.message.transform:SetParent(StudentMessagePanel.messageView)
    this.message.transform.localScale = Vector3.one

    LuaBehaviour:AddClick(this.message, this.OnMessageClick)
end

function StudentMessageCtrl.OnMessageClick(obj)
    print(obj.name)
end


-- Panel 开关
function StudentMessageCtrl.Open()
    StudentMessagePanel.panel.gameObject:SetActive(true)
end

function StudentMessageCtrl.Close()
    StudentMessagePanel.panel.gameObject:SetActive(false)
end