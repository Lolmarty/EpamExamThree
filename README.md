# EpamExamThree
1. Take up to 3 days to create an automated test based on Selenium 2 for the 1st chart on this page: 
http://www.highcharts.com/component/content/article/2-news/146-highcharts-5th-anniversary
2. Extract values for the green area chart called 'Highsoft employees' and verify them against values provided in the test program
3. Provide the test code and necessary documentation on how to install and run the test

>Extract values for the green area chart called 'Highsoft employees' and verify them against values provided in the test program

C какими именно данными искать соответствие того ,что на графике:
Количество сотрудников и последний присоединившийся.
Точнее, там сообщение всплывает: теперь нас столько-то, такой-то пришёл. Вот составить список таких значений и убедиться, что тест способен провести курсор так, чтобы они все повсплывали, и их корректно посчитывать.

Для запуска тестов в среде Visual Studio 2013 нужно
* создать новый пустой проект и добавить в него все файлы с расширением cs
* с помошью nuget установить:
  * NUnit framework & NUnit Test adapter
  * Selenium WebDriver & Selenium WebDriver Support Classes
  * ChromeDriver
* построить проект и в Test Explorer запустить тест.

Ожидаемым результатом есть возникновение окна браузера Google Chrome со страницей highcharts и последующим возникновением всплывающих сообщений вдоль графика Highsoft employees, после чего окно будет закрыто и тест будет пройден.
