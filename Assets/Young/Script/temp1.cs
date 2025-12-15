/*
    private void ResultDataUI()
    {
        GameResultData data = GameSession.CurrentResult;

        if (scoreText != null)
            scoreText.text = $"총 점수 : {data.totalScore}";

        if (killCountText != null)
            killCountText.text = $"처치 수 : {data.killCount}";

        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(data.playTime / 60F);
            int seconds = Mathf.FloorToInt(data.playTime % 60F);
            timeText.text = string.Format("플레이 시간 : {0:00}:{1:00}", minutes, seconds);
        }

        if (itemListText != null)
        {
            string itemStr = "";
            foreach (var item in data.obtainedItems)
            {
                // 아이템 이름과 수량 표시 (예: "포션 x 2")
                itemStr += $"{item.id} x {item.quantity}\n";
            }
            itemListText.text = itemStr;
        }
    }
*/