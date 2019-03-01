local transform
local gameObject

CoursePanel = {}
local this = CoursePanel

-- 启动事件
function CoursePanel.Awake(obj)
    gameObject = obj
    transform = obj.transform

    this.InitPanel()
    logWarn("Awake lua--->>"..gameObject.name);
end

function CoursePanel.InitPanel()
    this.panel = transform

    this.courseOptions = transform:Find('CourseOptions/Dropdown'):GetComponent('Dropdown')
    this.courseView = transform:Find('CourseView/Viewport/Content')
end

--单击事件--
function CoursePanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end