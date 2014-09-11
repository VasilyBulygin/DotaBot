while (isRunning) do
--обработка lowhp, отсылаем на базу
  if isLowHP then
    while isLowHP do
      if isDireTeam then
        coords_click(120,368,"right");
      else
        coords_click(10,471,"right");
      end;
    end;
  end;
--изучаем скиллы, если есть что изучать
 if dota_object_availible('LevelUpButton') then
   dota_object_click('LevelUpButton');
   Sleep(100);
   coords_click(317,447,'left');
   Sleep(100);
   coords_click(317+53,447,'left');
   Sleep(100);
   coords_click(317+53+53,447,'left');
 end;
--атакуем центральное строение 
--coords_click();--в атаку!

  sleep(1000);
end;