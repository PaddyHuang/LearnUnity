local transform
local gameObject

RecognisePanel = {}
local this = RecognisePanel

-- 启动事件
function RecognisePanel.Awake(obj)
	gameObject = obj
	transform = obj.transform
	
	this.InitPanel()	
end

-- 初始化面板
function RecognisePanel.InitPanel()
	this.recordBtn = transform:Find("RecordBtn").gameObject
	this.text = transform:Find("Text"):GetComponent("Text")	
	this.audioSource = transform:GetComponent("AudioSource")	
end

--单击事件--
function RecognisePanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end