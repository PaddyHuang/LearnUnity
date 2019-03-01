SearchCtrl = {}
local this = SearchCtrl

local transform
local gameObject
local LuaBehaviour

-- 构建函数
function SearchCtrl.New()   
    this.Awake() 
    return this
end

function SearchCtrl.Awake()    
    panelMgr:CreatePanel('Search', this.OnCreate)
end

-- 启动事件
function SearchCtrl.OnCreate(obj)
    gameObject = obj
    transform = obj.transform
    
    LuaBehaviour = transform:GetComponent('LuaBehaviour')    

    LuaBehaviour:AddClick(SearchPanel.backBtn, this.OnBackClick)
    LuaBehaviour:AddClick(SearchPanel.searchBtn, this.OnSearchClick)
    
    SearchPanel.dropdown.onValueChanged:AddListener(this.OnModelSelect)
       
    CtrlManager.CloseCtrl(this)
end

-- 点击事件
function SearchCtrl.OnBackClick(obj)
    print(obj.name)
end

function SearchCtrl.OnSearchClick(obj)
    print(obj.name)
end

function SearchCtrl.OnModelSelect(obj)
    print(obj.name)
end


-- Panel 开关
function SearchCtrl.Open()
    gameObject:SetActive(true)
end

function SearchCtrl.Close()
    gameObject:SetActive(false)
end