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
local maxRecordTime = 100		-- Max record time
local actualRecorTime = 0		-- Actual record time
local sampleRate = 16000		-- Sample Rate

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
	LuaBehaviour:AddClick(RecognisePanel.recordBtn, this.OnClick)		
	
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

-- 点击事件
function RecognisePanelCtrl.OnClick(go)
	print(go.name)
	RecognisePanel.text.text = go.name
end

-- 1. Start recording
function RecognisePanelCtrl.StartRecording()
	if not isHaveMic or Microphone.IsRecording(currentDeviceName) then
		print("No microphone found to record audio clip sample with.")
		return false	
	else
		Microphone.End(currentDeviceName)				-- Stop recording
		downTime = GetTimeStampOfNowWithMillisecond()	-- Markdown press time
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
