StudentPractiseCtrl = {}
local this = StudentPractiseCtrl

local transform
local gameObject
local LuaBehaviour

-- 构建函数
function StudentPractiseCtrl.New()   
    this.Awake() 
    return this
end

function StudentPractiseCtrl.Awake()    
    panelMgr:CreatePanel('StudentPractise', this.OnCreate)
end

-- 启动事件
function StudentPractiseCtrl.OnCreate(obj)
    gameObject = obj
    transform = obj.transform
    
    LuaBehaviour = transform:GetComponent('LuaBehaviour')    

    -- 之后改为动态添加
    LuaBehaviour:AddClick(StudentPractisePanel.yuWenCourseBtn, this.OnYuWenCourseClick)
    LuaBehaviour:AddClick(StudentPractisePanel.mathCourseBtn, this.OnMathCourseClick)
    LuaBehaviour:AddClick(StudentPractisePanel.physicsCourseBtn, this.OnPhysicsCourseClick)
    LuaBehaviour:AddClick(StudentPractisePanel.chemistryCourseBtn, this.OnChemistryCourseClick)
    LuaBehaviour:AddClick(StudentPractisePanel.EnglishCourseBtn, this.OnEnglishCourseClick)
    LuaBehaviour:AddClick(StudentPractisePanel.moreCourseBtn, this.OnMoreCourseClick)

    LuaBehaviour:AddClick(StudentPractisePanel.yuWenWrongBtn, this.OnYuWenWrongClick)
    LuaBehaviour:AddClick(StudentPractisePanel.mathWrongBtn, this.OnMathWrongClick)
    LuaBehaviour:AddClick(StudentPractisePanel.physicsWrongBtn, this.OnPhysicsWrongClick)
    LuaBehaviour:AddClick(StudentPractisePanel.chemistryWrongBtn, this.OnChemistryWrongClick)
    LuaBehaviour:AddClick(StudentPractisePanel.EnglishWrongBtn, this.OnEnglishWrongClick)
    LuaBehaviour:AddClick(StudentPractisePanel.moreWrongBtn, this.OnMoreWrongClick)
           
    CtrlManager.CloseCtrl(this)
end

-- 点击事件
function StudentPractiseCtrl.OnYuWenCourseClick(obj)
    print(obj.name)
end

function StudentPractiseCtrl.OnMathCourseClick(obj)
    print(obj.name)
end

function StudentPractiseCtrl.OnPhysicsCourseClick(obj)
    print(obj.name)
end

function StudentPractiseCtrl.OnChemistryCourseClick(obj)
    print(obj.name)
end

function StudentPractiseCtrl.OnEnglishCourseClick(obj)
    print(obj.name)
end

function StudentPractiseCtrl.OnMoreCourseClick(obj)
    print(obj.name)
end

function StudentPractiseCtrl.OnYuWenWrongClick(obj)
    print(obj.name)
end

function StudentPractiseCtrl.OnMathWrongClick(obj)
    print(obj.name)
end

function StudentPractiseCtrl.OnPhysicsWrongClick(obj)
    print(obj.name)
end

function StudentPractiseCtrl.OnChemistryWrongClick(obj)
    print(obj.name)
end

function StudentPractiseCtrl.OnEnglishWrongClick(obj)
    print(obj.name)
end

function StudentPractiseCtrl.OnMoreWrongClick(obj)
    print(obj.name)
end







-- Panel 开关
function StudentPractiseCtrl.Open()
    gameObject:SetActive(true)
end

function StudentPractiseCtrl.Close()
    gameObject:SetActive(false)
end