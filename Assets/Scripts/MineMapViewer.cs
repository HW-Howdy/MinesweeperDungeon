using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineMapViewer : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField]
	private ScrollRect _scrollRect;
	[SerializeField]
	private RectTransform _content;
	[SerializeField]
	private GameObject _cellPrefab;
	[SerializeField]
	private TMP_InputField _inputFieldCellSize;

	[Header("Grid Settings")]
	[SerializeField]
	private int cellSize = 100; // 각 칸 크기 (px)

	private int visibleRows;
	private int visibleCols;

	private List<GameObject> cellPool = new List<GameObject>();

	public void Start()
	{
		Resize(cellSize);
		_scrollRect.onValueChanged.AddListener((_) => UpdateCells());
		CellModel.ActionAfterClick += new Action(UpdateCellView);
		MineMapModel.Instance.ActionResizeMapAfter += Resize;
		return ;
	}

	public void ButtonResize()
	{
		if (!int.TryParse(_inputFieldCellSize.text, out int targetSize))
			targetSize = 50;
		Resize(targetSize);
		return ;
	}

	private void Resize()
	{
		Resize(cellSize);
		return ;
	}

	private void Resize(int targetSize)
	{
		cellSize = Math.Clamp(targetSize, 40, 200);
		_inputFieldCellSize.text = cellSize.ToString();
		// Content 크기를 전체 데이터 크기에 맞춤
		_content.sizeDelta = new Vector2(MineMapModel.Instance.Rows * cellSize, MineMapModel.Instance.Cols * cellSize);

		// Viewport에 보이는 셀 개수 계산 (+1 여유)
		RectTransform viewport = _scrollRect.viewport;
		visibleCols = Mathf.CeilToInt(viewport.rect.width / cellSize) + 1;
		visibleRows = Mathf.CeilToInt(viewport.rect.height / cellSize) + 1;

		// 풀링할 셀 생성
		int poolSize = visibleCols * visibleRows;
		int i = cellPool.Count;
		while (i < poolSize)
		{
			GameObject cell = Instantiate(_cellPrefab, _content);
			cellPool.Add(cell);
			i++;
		}
		while (--i > poolSize)
		{
			Destroy(cellPool[i]);
			cellPool.RemoveAt(i);
		}
		while (i >= 0)
		{
			cellPool[i--].GetComponent<RectTransform>().sizeDelta = Vector2.one * cellSize;
		}
		UpdateCells();
		return ;
	}

	private void UpdateCells()
	{
		// 스크롤된 위치 계산
		float scrollY = _content.anchoredPosition.y;
		float scrollX = -_content.anchoredPosition.x;

		int firstRow = Mathf.FloorToInt(scrollY / cellSize);
		int firstCol = Mathf.FloorToInt(scrollX / cellSize);

		for (int i = 0; i < cellPool.Count; i++)
		{
			int x = i / visibleCols + firstRow;
			int y = i % visibleCols + firstCol;
			GameObject cell = cellPool[i];

			if (x < 0 || x >= MineMapModel.Instance.Rows || y < 0 || y >= MineMapModel.Instance.Cols)
			{
				cell.SetActive(false);
				continue ;
			}
			cell.SetActive(true);
			RectTransform rt = cell.GetComponent<RectTransform>();
			rt.anchoredPosition = new Vector2(y * cellSize + cellSize / 2, - x * cellSize - cellSize / 2);
			cell.GetComponent<CellModel>().SetLocate(y, x);
		}
		return ;
	}

	public void UpdateCellView()
	{
		float scrollY = _content.anchoredPosition.y;
		float scrollX = -_content.anchoredPosition.x;

		int firstRow = Mathf.FloorToInt(scrollY / cellSize);
		int firstCol = Mathf.FloorToInt(scrollX / cellSize);

		for (int i = 0; i < cellPool.Count; i++)
		{
			int x = i / visibleCols + firstRow;
			int y = i % visibleCols + firstCol;
			if (x < 0 || x >= MineMapModel.Instance.Rows || y < 0 || y >= MineMapModel.Instance.Cols)
			{
				continue ;
			}
			cellPool[i].GetComponent<CellModel>().ReloadState(y, x);
		}
		return ;
	}
}
