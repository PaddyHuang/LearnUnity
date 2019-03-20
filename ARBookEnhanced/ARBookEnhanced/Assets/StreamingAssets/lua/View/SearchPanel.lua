local transform
local gameObject

SearchPanel = {}
local this = SearchPanel

-- 启动事件
function SearchPanel.Awake(obj)
    gameObject = obj
    transform = obj.transform

    this.InitPanel()
    logWarn("Awake lua--->>"..gameObject.name);
end

function SearchPanel.InitPanel()
    this.panel = transform

    this.backBtn = transform:Find('BackBtn').gameObject
    this.searchBtn = transform:Find('SearchBtn').gameObject
    this.dropdown = transform:Find('Dropdown'):GetComponent('Dropdown')
    this.inputField = transform:Find('InputField'):GetComponent('InputField')
end

--单击事件--
function SearchPanel.OnDestroy()
	logWarn("OnDestroy---->>>");
end