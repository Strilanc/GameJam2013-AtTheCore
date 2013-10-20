using UnityEngine;
using System.Collections;

public interface DataDelegate {

	void ProcessBytes(byte[] bytes, int byteSize);
}
