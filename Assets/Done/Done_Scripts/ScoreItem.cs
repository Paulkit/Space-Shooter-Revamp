using System;
using System.Xml;
using System.Xml.Serialization;

public class ScoreItem {

    [XmlAttribute("rank")]
    public string rank;

    [XmlElement("Date")]
    public string date;

    [XmlElement("Score")]
    public int score;

    public override string ToString()
    {
        return rank + ". " + date + "\tScore: " + score;
    }

    public void updateScore(int _score, string _date)
    {
        if (_date.Equals(""))
        {
            DateTime localDate = DateTime.Now;
            _date = localDate.ToString("yyyy-MM-dd HH:mm:ss");
        }
        score = _score;
        date = _date;  
    }
}
