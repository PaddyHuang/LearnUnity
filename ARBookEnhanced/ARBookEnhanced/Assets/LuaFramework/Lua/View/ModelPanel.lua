local transform
local gameObject

ModelPanel = {}
local this = ModelPanel

-- 启动事件
function ModelPanel.Awake(obj)
    gameObject = obj
    transform = obj.transform

    this.InitPanel()
    logWarn("Awake lua--->>"..gameObject.name);
end

function ModelPanel.InitPanel()
    this.panel = transform

    this.backBtn = transform:Find('BackBtn').gameObject
    
end

--单击事件--
function ModelPanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end