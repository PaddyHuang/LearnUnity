local transform
local gameObject

NavigationPanel = {}
local this = NavigationPanel

-- 启动事件
function NavigationPanel.Awake(obj)
    gameObject = obj
    transform = obj.transform

    this.InitPanel()
    logWarn("Awake lua--->>"..gameObject.name);
end

function NavigationPanel.InitPanel()
    this.panel = transform
    
    this.courseBtn = transform:Find('CourseBtn').gameObject
    this.practiseBtn = transform:Find('PractiseBtn').gameObject
    this.messageBtn = transform:Find('MessageBtn').gameObject
    this.userBtn = transform:Find('UserBtn').gameObject
    
end

--单击事件--
function NavigationPanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end