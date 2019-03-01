local transform
local gameObject

DetialPanel = {}
local this = DetialPanel

-- 启动事件
function DetialPanel.Awake(obj)
    gameObject = obj
    transform = obj.transform

    this.InitPanel()
    logWarn("Awake lua--->>"..gameObject.name);
end

function DetialPanel.InitPanel()
    this.searchBtn = transform:Find('SearchBtn').gameObject
    this.backBtn = transform:Find('BackBtn').gameObject
    this.courseName = transform:Find('CourseName'):GetComponent('Text')
    this.introduce = transform:Find('Introduce'):GetComponent('Text')
    this.arBtn = transform:Find('ARBtn').gameObject
    this.chapterView = transform:Find('ChapterView/Viewport/Content')
end

--单击事件--
function DetialPanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end