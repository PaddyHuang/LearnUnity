local transform
local gameObject

StudentPractisePanel = {}
local this = StudentPractisePanel

-- 启动事件
function StudentPractisePanel.Awake(obj)
    gameObject = obj
    transform = obj.transform

    this.InitPanel()
    logWarn("Awake lua--->>"..gameObject.name);
end

function StudentPractisePanel.InitPanel()
    this.yuWenCourseBtn = transform:Find('Courses/YuWenCourses').gameObject
    this.mathCourseBtn = transform:Find('Courses/MathCourses').gameObject
    this.physicsCourseBtn = transform:Find('Courses/PhysicsCourses').gameObject
    this.chemistryCourseBtn = transform:Find('Courses/ChemistryCourses').gameObject
    this.EnglishCourseBtn = transform:Find('Courses/EnglishCourses').gameObject
    this.moreCourseBtn = transform:Find('Courses/MoreCourses').gameObject
    
    this.yuWenWrongBtn = transform:Find('Wrongs/YuWenWrongs').gameObject
    this.mathWrongBtn = transform:Find('Wrongs/MathWrongs').gameObject
    this.physicsWrongBtn = transform:Find('Wrongs/PhysicsWrongs').gameObject
    this.chemistryWrongBtn = transform:Find('Wrongs/ChemistryWrongs').gameObject
    this.EnglishWrongBtn = transform:Find('Wrongs/EnglishWrongs').gameObject
    this.moreWrongBtn = transform:Find('Wrongs/MoreWrongs').gameObject
    
end

--单击事件--
function StudentPractisePanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end