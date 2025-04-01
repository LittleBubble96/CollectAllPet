using System;

public class ConfBTCShape
{
    public Type BTNodeType;
    public string Description;
    public string[] StringParams;
    public ConfBTCShape[] Decorators;
    public ConfBTCShape[] Services;
    public ConfBTCShape[] Children;
    public bool Inversed = false;
}