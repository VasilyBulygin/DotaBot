function main()
while (isRunning) do
--[[обработка lowhp, отсылаем на базу
  if isLowHP then
    while isLowHP do
      if isDireTeam then
        coords_click(120,368,"right");
      else
        coords_click(10,471,"right");
      end;
    end;
  end;
--]]
--изучаем скиллы, если есть что изучать
 if b_dota_object_availible('LevelUpButton') then
   b_dota_object_click('LevelUpButton','left');
   b_sleep(100);
   b_coords_click(317,447,'left');
   b_sleep(100);
   b_coords_click(317+53,447,'left');
   b_sleep(100);
   b_coords_click(317+53+53,447,'left');
 end;
--атакуем центральное строение 
--coords_click();--в атакуe!
b_coords_click(14,467,'right');
b_sleep(1000);

end;
end;
main();