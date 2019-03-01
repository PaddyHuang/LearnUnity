SignupCtrl = {}
local this = SignupCtrl

local transform
local gameObject
local LuaBehaviour

local timer     -- 计时器
local count     -- 计数器

-- 构建函数
function SignupCtrl.New()    
    this.Awake()
    return this
end

function SignupCtrl.Awake()    
    panelMgr:CreatePanel('Signup', this.OnCreate)
end

-- 启动事件
function SignupCtrl.OnCreate(obj)
    gameObject = obj
    transform = obj.transform
    
    LuaBehaviour = transform:GetComponent('LuaBehaviour')    

    LuaBehaviour:AddClick(SignupPanel.signupBtn, this.OnSignupClick)
    LuaBehaviour:AddClick(SignupPanel.doneBtn, this.OnDoneClick)

    timer = Timer.New(this.OnTimerCallFunc, 1, -1, true)
        
    CtrlManager.CloseCtrl(this)
end

-- 监听事件
function SignupCtrl.OnSignupClick()
    local user = UserManager.Instance:CreateUser()
        
    if UserManager.Instance:SignupUser(user, SignupPanel.phone.text, SignupPanel.name.text, SignupPanel.account.text, SignupPanel.password.text,
                                        SignupPanel.occupation.value, SignupPanel.school.text, SignupPanel.class.text, SignupPanel.inviteCode.text, 
                                        SignupPanel.email.text) then        
        -- UserManager.Instance:TravelUsers()   // 遍历，看看User是否添加到本地        
        SignupPanel.donePanel.gameObject:SetActive(true)
        count = 5
        timer:Start()
    end   
end

function SignupCtrl.OnDoneClick()    
    if SignupPanel.occupation.value == 0 then
        sceneMgr.LoadSceneByIndex(1)            -- 登录学生端
    end

    if SignupPanel.occupation.value == 1 then
        sceneMgr.LoadSceneByIndex(2)            -- 登录教师端
    end

end

-- 计时器回调
function SignupCtrl.OnTimerCallFunc()    
    SignupPanel.doneText.text = string.gsub( SignupPanel.doneText.text,tostring(count),count - 1)
    count = count - 1
	
	if count == 0 then
        count = 5
        timer:Stop()
        this.OnDoneClick()  --  加载场景
	end
end

-- Panel 开关
function SignupCtrl.Open()
    gameObject:SetActive(true)
end

function SignupCtrl.Close()
    SignupPanel.donePanel.gameObject:SetActive(false)    -- 关闭注册成功面板
    gameObject:SetActive(false)
end