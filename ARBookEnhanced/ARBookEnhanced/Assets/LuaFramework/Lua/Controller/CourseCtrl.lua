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
    
    CoursePanel.courseOptions.onValueChanged:AddListener(this.OnCourseSelect)

    -- 加载课程，之后改成按Json动态添加
    resMgr:LoadPrefab('Course', {'Course'}, this.OnLoadCourseFinish) 
end

-- 点击事件
function CourseCtrl.OnCourseSelect(obj)
    print(obj)
end

-- 加载回调
function CourseCtrl.OnLoadCourseFinish(obj)
    this.yuWenBtn = GameObject.Instantiate(obj[0])
    
    this.yuWenBtn.transform:SetParent(CoursePanel.courseView)
    this.yuWenBtn.transform.localScale = Vector3.one

    LuaBehaviour:AddClick(this.yuWenBtn, this.OnYuWenClick)
end

function CourseCtrl.OnYuWenClick(obj)
    print(obj.name)
end


-- Panel 开关
function CourseCtrl.Open()
    CoursePanel.panel.gameObject:SetActive(true)
end

function CourseCtrl.Close()
    CoursePanel.panel.gameObject:SetActive(false)
end