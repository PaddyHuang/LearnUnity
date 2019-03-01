StudentUserCtrl = {}
local this = StudentUserCtrl

local transform
local gameObject
local LuaBehaviour

-- 构建函数
function StudentUserCtrl.New()   
    this.Awake() 
    return this
end

function StudentUserCtrl.Awake()    
    panelMgr:CreatePanel('StudentUser', this.OnCreate)
end

-- 启动事件
function StudentUserCtrl.OnCreate(obj)
    gameObject = obj
    transform = obj.transform
    
    LuaBehaviour = transform:GetComponent('LuaBehaviour')    

    LuaBehaviour:AddClick(StudentUserPanel.responseBtn, this.OnResponseClick)

    -- 之后改为动态添加
    LuaBehaviour:AddClick(StudentUserPanel.yuWenCourseBtn, this.OnYuWenCourseClick)
    LuaBehaviour:AddClick(StudentUserPanel.mathCourseBtn, this.OnMathCourseClick)
    LuaBehaviour:AddClick(StudentUserPanel.physicsCourseBtn, this.OnPhysicsCourseClick)
    LuaBehaviour:AddClick(StudentUserPanel.chemistryCourseBtn, this.OnChemistryCourseClick)
    LuaBehaviour:AddClick(StudentUserPanel.EnglishCourseBtn, this.OnEnglishCourseClick)
    LuaBehaviour:AddClick(StudentUserPanel.moreCourseBtn, this.OnMoreCourseClick)

    LuaBehaviour:AddClick(StudentUserPanel.yuWenWrongBtn, this.OnYuWenWrongClick)
    LuaBehaviour:AddClick(StudentUserPanel.mathWrongBtn, this.OnMathWrongClick)
    LuaBehaviour:AddClick(StudentUserPanel.physicsWrongBtn, this.OnPhysicsWrongClick)
    LuaBehaviour:AddClick(StudentUserPanel.chemistryWrongBtn, this.OnChemistryWrongClick)
    LuaBehaviour:AddClick(StudentUserPanel.EnglishWrongBtn, this.OnEnglishWrongClick)
    LuaBehaviour:AddClick(StudentUserPanel.moreWrongBtn, this.OnMoreWrongClick)

    CtrlManager.CloseCtrl(this)           
end

-- 点击事件
function StudentUserCtrl.OnResponseClick(obj)
    print(obj.name)
end

function StudentUserCtrl.OnYuWenCourseClick(obj)
    print(obj.name)
end

function StudentUserCtrl.OnMathCourseClick(obj)
    print(obj.name)
end

function StudentUserCtrl.OnPhysicsCourseClick(obj)
    print(obj.name)
end

function StudentUserCtrl.OnChemistryCourseClick(obj)
    print(obj.name)
end

function StudentUserCtrl.OnEnglishCourseClick(obj)
    print(obj.name)
end

function StudentUserCtrl.OnMoreCourseClick(obj)
    print(obj.name)
end

function StudentUserCtrl.OnYuWenWrongClick(obj)
    print(obj.name)
end

function StudentUserCtrl.OnMathWrongClick(obj)
    print(obj.name)
end

function StudentUserCtrl.OnPhysicsWrongClick(obj)
    print(obj.name)
end

function StudentUserCtrl.OnChemistryWrongClick(obj)
    print(obj.name)
end

function StudentUserCtrl.OnEnglishWrongClick(obj)
    print(obj.name)
end

function StudentUserCtrl.OnMoreWrongClick(obj)
    print(obj.name)
end







-- Panel 开关
function StudentUserCtrl.Open()
    gameObject:SetActive(true)
end

function StudentUserCtrl.Close()
    gameObject:SetActive(false)
end