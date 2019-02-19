require "Common/define"
require "Common/GameData"

RecognisePanelCtrl = {}
local this = RecognisePanelCtrl

local panel
local LuaBehaviour
local transform
local gameObject

local downTime = 0				-- Duration of press
local isHaveMic = false			-- Mark poccession of any microphone
local currentDeviceName = ""	-- Devices name
local maxRecordTime = 10		-- Max record time
local actualRecorTime = 0		-- Actual record time
local sampleRate = 16000		-- Sample Rate

local timer 
local counter

-- 构建函数
function RecognisePanelCtrl.New()
	return this
end

function RecognisePanelCtrl.Awake()
	panelMgr:CreatePanel('Recognise', this.OnCreate)
end

-- 启动事件
function RecognisePanelCtrl.OnCreate(obj)
	gameObject = obj
	transform = obj.transform
	
	LuaBehaviour = transform:GetComponent('LuaBehaviour')

	-- LuaBehaviour:AddClick(RecognisePanel.recordBtn, this.OnClick)		
	EventTriggerListener.Get(RecognisePanel.recordBtn).onDown = this.OnPointerDown
	EventTriggerListener.Get(RecognisePanel.recordBtn).onUp = this.OnPointerUp

	timer = Timer.New(this.OnTimerCallFunc, 0.01, -1, true)
	
	-- print(Microphone.devices[0])
	this.InitMicrophone();
end

-- Init Microphone
function RecognisePanelCtrl.InitMicrophone()
	if Microphone.devices.Length > 0 then
		isHaveMic = true
		currentDeviceName = Microphone.devices[0]
	end
end

-- Timer Recall Func
function RecognisePanelCtrl.OnTimerCallFunc()
	counter = counter + 0.01	
	
	if counter >= maxRecordTime then 
		counter = 0
	end	
end

-- 点击事件
function RecognisePanelCtrl.OnPointerDown()		
	counter = 0	
	-- timer:Start()	
	print(tostring(this.GetTimeStampOfNowWithMillisecond()))
	
	if this.StartRecording() then
		RecognisePanel.text = '录音成功'
	else
		RecognisePanel.text = '录音失败'
	end
end

function RecognisePanelCtrl.OnPointerUp()		
	-- actualRecorTime = EndRecording()
		
	if actualRecorTime > 1 then
		RecognisePanel.audioSource:PlayOnShot(audioSource.clip)
		coroutine.start(this.AsrData)
	else
		RecognisePanel.text = '录音时长过短'
	end
end

-- 1. Start recording
function RecognisePanelCtrl.StartRecording()
	if not isHaveMic or Microphone.IsRecording(currentDeviceName) then
		print("No microphone found to record audio clip sample with.")
		return false	
	else
		Microphone.End(currentDeviceName)				-- Stop recording
		-- downTime = GetTimeStampOfNowWithMillisecond()	-- Markdown press time
		downTime = counter		
		-- Start recording
		RecognisePanel.audioSource.clip = Microphone.Start(currentDeviceName, false, maxRecordTime, sampleRate)
		
		return true
	end	
end

-- 2. End recording
function RecognisePanelCtrl.EndRecording()
	if not isHaveMic or not Microphone.IsRecording(currentDeviceName) then
		return 0
	end
		
	Microphone.End(currentDeviceName)
	return Mathf.CeilToInt((GetTimeStampOfNowWithMillisecond - downTime) / 1000)	
end

-- Get time stamp of now with millisecond
function RecognisePanelCtrl.GetTimeStampOfNowWithMillisecond()
	return (System.DateTime.Now:ToUniversalTime().Ticks - 621355968000000000) / 10000
	-- return (Util.GetTime() - 621355968000000000) / 10000
end

-- 百度接口
function AsrData()
	local samples = Util.Float(SampleRate * TRUE_RECORD_TIME * audioSource.clip.channels)
	
end
