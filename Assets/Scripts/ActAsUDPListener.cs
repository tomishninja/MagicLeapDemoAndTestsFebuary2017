using NetworkingLibaryStandard;
using UnityEngine;

public class ActAsUDPListener : MonoBehaviour, IDisplayMessage
{

    public int ExpectedAmountOfDataPerPackage = 7;

    public int portNumber = UDPListener.DefaultPortNumber;

    public UpdatePosition[] objectArray = new UpdatePosition[0];

    public static object obj = new object();
    // Use this for initialization
    void Start()
    {
        UDPListener listener = new UDPListener(portNumber, this);
        listener.Start();
    }

    public virtual void DisplayMessage(MessageHelper.MessageType type, string message)
    {
        if (objectArray != null && objectArray.Length != 0 && MessageHelper.MessageType.Data == type)
        {
            lock (obj)
            {
                // parse the message
                // in this case remove all white space and split via the commas
                string[] data = message.Replace(" ", "").Split(',');
                if (data.Length / objectArray.Length == ExpectedAmountOfDataPerPackage)
                {
                    int dataIndex = 0;

                    // insert the data were it needs to go
                    for (int i = 0; i < objectArray.Length; i++)
                    {
                        dataIndex = 0;

                        objectArray[i].SetNewRotation(new Quaternion(
                            int.Parse(data[dataIndex++]),
                            int.Parse(data[dataIndex++]),
                            int.Parse(data[dataIndex++]),
                            int.Parse(data[dataIndex++])
                            ));

                        objectArray[i].SetNewPos(new Vector3(
                            int.Parse(data[dataIndex++]),
                            int.Parse(data[dataIndex++]),
                            int.Parse(data[dataIndex++])
                            ));
                    }
                }
            }
        }
    }
}
