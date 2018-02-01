using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

[XmlRoot("ScoreRecord")]
public class ScoreList
{
    [XmlArray("ScoreList")]
    [XmlArrayItem("ScoreItem")]
    public List<ScoreItem> scoreList = new List<ScoreItem>();

    public string updateScoreList(int score)
    {
        int preScore = 0, tempScore = 0;
        string preDate = "", tempDate = "";
        bool updater = false;

        foreach (ScoreItem _scoreItem in scoreList)
        {
            if (updater == true)
            {
                tempScore = _scoreItem.score;
                tempDate = _scoreItem.date;
                _scoreItem.updateScore(preScore, preDate);
                preScore = tempScore;
                preDate = tempDate;
            }
            else if (score > _scoreItem.score)
            {
                preScore = _scoreItem.score;
                preDate = _scoreItem.date;
                _scoreItem.updateScore(score, "");
                updater = true;
            }
        }

        string str = "";
        foreach (ScoreItem _scoreItem in scoreList)
        {
            str = str + _scoreItem.ToString() + "\n";
        }

        return str;
    }

    public void xmlCreator(string path)
    {
        new XDocument(
        new XElement("ScoreRecord",
        new XElement("ScoreList",
        new XElement("ScoreItem", new XAttribute("rank", "1"),
        new XElement("Date", "2016-01-01 00:00:00"),
        new XElement("Score", "0")
        ),
        new XElement("ScoreItem", new XAttribute("rank", "2"),
        new XElement("Date", "2016-01-01 00:00:00"),
        new XElement("Score", "0")
        )
        ,
        new XElement("ScoreItem", new XAttribute("rank", "3"),
        new XElement("Date", "2016-01-01 00:00:00"),
        new XElement("Score", "0")
        )
        ,
        new XElement("ScoreItem", new XAttribute("rank", "4"),
        new XElement("Date", "2016-01-01 00:00:00"),
        new XElement("Score", "0")
        )
        ,
        new XElement("ScoreItem", new XAttribute("rank", "5"),
        new XElement("Date", "2016-01-01 00:00:00"),
        new XElement("Score", "0")
        )))).Save(path);
    }

}
