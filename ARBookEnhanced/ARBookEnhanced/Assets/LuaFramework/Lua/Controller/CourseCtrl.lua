CourseCtrl = {}
local this = CourseCtrl

local transform
local gameObject
local LuaBehaviour

-- 构建函数
function CourseCtrl.New()   
    this.Awake() 
    return this
end

function CourseCtrl.Awake()    
    panelMgr:CreatePanel('Course', this.OnCreate)
end

-- 启动事件
function CourseCtrl.OnCreate(obj)
    gameObject = obj
    transform = obj.transform
    
    LuaBehaviour = transform:GetComponent('LuaBehaviour')    

    LuaBehaviour:AddClick(CoursePanel.CourseBtn, this.OnCourseClick)
    
    CoursePanel.gradeOptions.onValueChanged:AddListener(this.OnGradeSelect)

    -- 加载课程，之后改成按Json动态添加
    resMgr:LoadPrefab('Course', {'Course'}, this.OnLoadCourseFinish) 
end

-- 点击事件
function CourseCtrl.OnGradeSelect(obj)
    print(obj)
end

-- 加载回调
function CourseCtrl.OnLoadCourseFinish(obj)
    this.courseBtn = GameObject.Instantiate(obj[0])
    
    this.courseBtn.transform:SetParent(CoursePanel.courseView)
    this.courseBtn.transform.localScale = Vector3.one

    LuaBehaviour:AddClick(this.courseBtn, this.OnCourseClick)
end

function CourseCtrl.OnCourseClick()
    CtrlManager.OpenCtrl(CtrlNames.Detial)
end


-- Panel 开关
function CourseCtrl.Open()
    CoursePanel.panel.gameObject:SetActive(true)
end

function CourseCtrl.Close()
    CoursePanel.panel.gameObject:SetActive(false)
end