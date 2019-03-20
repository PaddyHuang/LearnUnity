DetialCtrl = {}
local this = DetialCtrl

local transform
local gameObject
local LuaBehaviour

-- 构建函数
function DetialCtrl.New()   
    this.Awake() 
    return this
end

function DetialCtrl.Awake()    
    panelMgr:CreatePanel('Detial', this.OnCreate)
end

-- 启动事件
function DetialCtrl.OnCreate(obj)
    gameObject = obj
    transform = obj.transform
    
    LuaBehaviour = transform:GetComponent('LuaBehaviour')    

    LuaBehaviour:AddClick(DetialPanel.backBtn, this.OnBackClick)
    LuaBehaviour:AddClick(DetialPanel.searchBtn, this.OnSearchClick)
    LuaBehaviour:AddClick(DetialPanel.arBtn, this.OnARClick)
        
    -- 加载章节，之后改成按Json动态添加
    resMgr:LoadPrefab('Chapter', {'Chapter'}, this.OnLoadChapterFinish) 

    CtrlManager.CloseCtrl(this)
end

-- 点击事件
function DetialCtrl.OnBackClick()
    CtrlManager.OpenCtrl(CtrlNames.Course)
end

function DetialCtrl.OnSearchClick(obj)
    CtrlManager.OpenCtrl(CtrlNames.Search)
end

function DetialCtrl.OnARClick(obj)
    print(obj.name)
end

-- 加载回调
function DetialCtrl.OnLoadChapterFinish(obj)
    this.chapter = GameObject.Instantiate(obj[0])
    
    this.chapter.transform:SetParent(DetialPanel.chapterView)
    this.chapter.transform.localScale = Vector3.one

    LuaBehaviour:AddClick(this.chapter, this.OnChapterlick)
end

function DetialCtrl.OnChapterlick(obj)
    print(obj.name)
end


-- Panel 开关
function DetialCtrl.Open()
    DetialPanel.panel.gameObject:SetActive(true)
end

function DetialCtrl.Close()
    DetialPanel.panel.gameObject:SetActive(false)
end