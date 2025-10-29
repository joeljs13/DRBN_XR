using UnityEngine;

public class DomainProperties : MonoBehaviour
{
    public float hydrophobicity = 0;
    public float area = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public float getHydrophobicity()
    {
        return hydrophobicity;
    }
    
    public void setHydrophobicity(float newValue)
    {
        hydrophobicity = newValue;
    }
    public float getArea()
    {
        return area;
    }
    
    public void setArea(float newValue)
    {
        area = newValue;
    }
}
