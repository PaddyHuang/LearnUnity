local transform
local gameObject

StudentMessagePanel = {}
local this = StudentMessagePanel

-- 启动事件
function StudentMessagePanel.Awake(obj)
    gameObject = obj
    transform = obj.transform

    this.InitPanel()
    logWarn("Awake lua--->>"..gameObject.name);
end

function StudentMessagePanel.InitPanel()
    this.messageView = transform:Find('MessageView'):GetComponent('ScrollRect')
end

--单击事件--
function StudentMessagePanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end